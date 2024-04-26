using System.Collections;
using UnityEngine;

namespace HearthsNeighbor
{
    /// <summary>
    /// Script that enables the button for the ending sequence
    /// </summary>
    public class FinalButton : MonoBehaviour
    {
        private Animator anim;
        private GameObject signal;
        private SingleInteractionVolume interact;

        private void Awake()
        {
            anim = GetComponentInParent<Animator>();
            signal = GameObject.Find("FinalSignal");
            signal.SetActive(false);
            interact = this.GetRequiredComponent<SingleInteractionVolume>();
            interact.OnPressInteract += OnPressInteract;
            interact.EnableInteraction();
        }

        private void Start()
        {
            if (HearthsNeighbor.GetIsMultiplayer())
            {
                HearthsNeighbor.Main.OnEndingButtonPress += OnPushButtonQSB;
            }
        }

        private void OnDestroy()
        {
            if (HearthsNeighbor.GetIsMultiplayer())
            {
                HearthsNeighbor.Main.OnEndingButtonPress -= OnPushButtonQSB;
            }
        }

        private void OnPushButtonQSB(uint playerID, bool bleh)
        {
            EndingButton(false);
        }

        private void OnPressInteract()
        {
            EndingButton(true);
        }

        private void EndingButton(bool activatedLocally)
        {
            anim.SetTrigger("ButtonPress");
            interact.DisableInteraction();
            signal.SetActive(true);
            if (HearthsNeighbor.GetIsMultiplayer() && activatedLocally)
            {
                HearthsNeighbor.Main.qsb.SendMessage<bool>("HN1EndingButton", true);
            }
        }
    }
}