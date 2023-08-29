using System.Collections;
using UnityEngine;

namespace HearthsNeighbor
{
    public class ElevatorButton : MonoBehaviour
    {
        public Elevator MyElevator;

        private SingleInteractionVolume _interactVolume;
        private Animator anim;
        
        private void Awake()
        {
            _interactVolume = this.GetRequiredComponent<SingleInteractionVolume>();
            _interactVolume.OnPressInteract += OnPressInteract;
            anim = GetComponent<Animator>();
        }

        private void OnPressInteract()
        {
            MyElevator.StartTransport();
            //do animation
        }
    }
}