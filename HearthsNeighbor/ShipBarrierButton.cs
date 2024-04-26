using System.Collections;
using UnityEngine;

namespace HearthsNeighbor
{
    public class ShipBarrierButton : MonoBehaviour
    {
        public ShipBarrierPanel Panel;
        public AudioSource Beep;
        [Range(0, 4)]
        public int ID;

        private Animator anim;
        private SingleInteractionVolume interact;

        private void Awake()
        {
            anim = GetComponentInParent<Animator>();
            interact = this.GetRequiredComponent<SingleInteractionVolume>();
        }

        private void Start()
        {
            interact.OnPressInteract += OnPressInteract;
            interact.EnableInteraction();
            interact.SetPromptText(UITextType.PressPrompt);
            if (HearthsNeighbor.GetIsMultiplayer())
            {
                HearthsNeighbor.Main.OnShipKeypadPress += OnPushButtonQSB;
            }
        }

        private void OnDestroy()
        {
            if (HearthsNeighbor.GetIsMultiplayer())
            {
                HearthsNeighbor.Main.OnShipKeypadPress -= OnPushButtonQSB;
            }
        }

        private void OnPushButtonQSB(uint playerID, short receivedID)
        {
            if (receivedID == ID)
            {
                PressKeypadButton(false);
            }
        }

        private void OnPressInteract()
        {
            PressKeypadButton(true);
        }

        private void PressKeypadButton(bool activatedLocally)
        {
            Beep.Play();
            anim.SetTrigger("PressButton");
            Panel.InputDigit(ID);
            if (HearthsNeighbor.GetIsMultiplayer() && activatedLocally)
            {
                HearthsNeighbor.Main.qsb.SendMessage<short>("HN1ShipKeypad", (short)ID);
            }
            StartCoroutine(ButtonDelay());
        }

        private IEnumerator ButtonDelay()
        {
            interact.DisableInteraction();
            yield return new WaitForSeconds(2);
            interact.EnableInteraction();
        }
    }
}