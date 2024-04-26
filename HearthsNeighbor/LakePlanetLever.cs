using System.Collections;
using UnityEngine;

namespace HearthsNeighbor
{
    public class LakePlanetLever : MonoBehaviour
    {
        public bool isOn;

        private SingleInteractionVolume interaction;
        private GameObject lakeDoor;
        private Animator anim;

        private void Awake()
        {
            interaction = GetComponent<SingleInteractionVolume>();
            interaction.OnPressInteract += ToggleDoorEvent;
            interaction.enabled = true;
            interaction.EnableInteraction();

            anim = GetComponentInParent<Animator>();
        }

        private void Start()
        {
            if (HearthsNeighbor.GetIsMultiplayer())
            {
                HearthsNeighbor.Main.OnLeverActivate += OnPullLeverQSB;
            }
        }

        private void OnDestroy()
        {
            if (HearthsNeighbor.GetIsMultiplayer())
            {
                HearthsNeighbor.Main.OnLeverActivate -= OnPullLeverQSB;
            }
        }

        private void OnPullLeverQSB(uint playerID, bool pulled)
        {
            ToggleDoor(false);
        }

        private void ToggleDoorEvent()
        {
            ToggleDoor(true);
        }

        private void ToggleDoor(bool activatedLocally)
        {
            isOn = !isOn;
            anim.SetBool("IsOn", isOn);
            if (lakeDoor == null)
            {
                HearthsNeighbor.LogMessage($"Main planet is {HearthsNeighbor.Main.MainPlanet.gameObject.name}");
                lakeDoor = HearthsNeighbor.Main.MainPlanet.transform.Find("Sector/MainPlanet/Sectors/Lake/LakeDoor").gameObject;

                interaction.DisableInteraction();
            }
            lakeDoor.SetActive(!isOn);
            if (HearthsNeighbor.GetIsMultiplayer() && activatedLocally)
            {
                HearthsNeighbor.Main.qsb.SendMessage<bool>("HN1PullLever", true);
            }
        }
    }
}