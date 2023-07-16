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
        }

        private void OnPressInteract()
        {
            Beep.Play();
            anim.SetTrigger("PressButton");
            Panel.InputDigit(ID);
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