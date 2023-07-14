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

        private void OnPressInteract()
        {
            if(!isOn)
            {
                buttonSprite.color = brightColor;
                blockade.ActivateButton(myID);
                isOn = true;
            }
            interaction.DisableInteraction();
        }
    }
}