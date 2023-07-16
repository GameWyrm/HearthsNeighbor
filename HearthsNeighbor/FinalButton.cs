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

        private void OnPressInteract()
        {
            anim.SetTrigger("ButtonPress");
            interact.DisableInteraction();
            signal.SetActive(true);
        }
    }
}