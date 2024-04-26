using System.Collections;
using UnityEngine;

namespace HearthsNeighbor
{
    /// <summary>
    /// Button that connects to the fire shrine blockade and turn it off when all buttons are on
    /// </summary>
    public class FireShrineButton : MonoBehaviour
    {
        public SpriteRenderer buttonSprite;
        public bool isOn;
        public FireShrineGate blockade;
        public Color dullColor;
        public Color brightColor;
        public int myID;

        private SingleInteractionVolume interaction;

        private void Awake()
        {
            interaction = this.GetRequiredComponent<SingleInteractionVolume>();
            interaction.SetPromptText(UITextType.PushPrompt);
            interaction.OnPressInteract += OnPressInteract;
            interaction.EnableInteraction();

            buttonSprite = GetComponent<SpriteRenderer>();
            buttonSprite.color = dullColor;
        }

        private void Start()
        {
            if (HearthsNeighbor.GetIsMultiplayer())
            {
                HearthsNeighbor.Main.OnLavaButtonPress += OnPushButtonQSB;
            }
        }

        private void OnDestroy()
        {
            if (HearthsNeighbor.GetIsMultiplayer())
            {
                HearthsNeighbor.Main.OnLavaButtonPress -= OnPushButtonQSB;
            }
        }

        private void OnPushButtonQSB(uint playerID, short ID)
        {
            if (ID == myID)
            {
                PushButton(false);
            }
        }

        private void OnPressInteract()
        {
            PushButton(true);
            interaction.DisableInteraction();
        }

        private void PushButton(bool activatedLocally)
        {
            if (!isOn)
            {
                buttonSprite.color = brightColor;
                blockade.ActivateButton(myID);
                if (HearthsNeighbor.GetIsMultiplayer() && activatedLocally)
                {
                    HearthsNeighbor.Main.qsb.SendMessage<short>("HN1PushLavaButton", (short)myID);
                }
                isOn = true;
            }
        }
    }
}