using System.Collections;
using UnityEngine;

namespace OuterWildsSummerJam
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
            interaction.OnPressInteract += ToggleDoor;
            interaction.enabled = true;
            interaction.EnableInteraction();

            anim = GetComponentInParent<Animator>();
        }

        private void ToggleDoor()
        {
            isOn = !isOn;
            anim.SetBool("IsOn", isOn);
            if (lakeDoor == null)
            {
                OuterWildsSummerJam.LogMessage($"Main planet is {OuterWildsSummerJam.Main.MainPlanet.gameObject.name}");
                lakeDoor = OuterWildsSummerJam.Main.MainPlanet.transform.Find("Sector/MainPlanet/Sectors/Lake/LakeDoor").gameObject;
            }
            lakeDoor.SetActive(!isOn);
        }
    }
}