using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HearthsNeighbor
{
    public class PressurePlateDoor : MonoBehaviour
    {
        /// <summary>
        /// List of connected pressure plates
        /// </summary>
        public List<PressurePlate> powerSources;
        /// <summary>
        /// If true, this door will close if it loses power. By default, once doors open they'll stay on forever.
        /// </summary>
        public bool RequireConstantPower = false;

        private Animator anim;
        private List<PressurePlate> powered;

        private void Awake()
        {
            anim = GetComponentInParent<Animator>();
            powerSources = new();
            powered = new();
        }

        /// <summary>
        /// Adds a PowerSource to be managed by this controller
        /// </summary>
        /// <param name="source"></param>
        public void RegisterPowerSource(PressurePlate source)
        {
            powerSources.Add(source);
        }

        /// <summary>
        /// Adds a source to a list of sources that are on. Once all sources are on, powers the door.
        /// </summary>
        /// <param name="source"></param>
        public void PowerSource(PressurePlate source)
        {
            if (!powered.Contains(source))
            {
                powered.Add(source);
            }
            CheckPower();
        }

        /// <summary>
        /// Removes a source from the list of powered sources. Only does anything if the door isn't already powered.
        /// </summary>
        /// <param name="source"></param>
        public void UnpowerSource(PressurePlate source)
        {
            if (powered.Contains(source))
            {
                powered.Remove(source);
            }
            CheckPower();
        }

        /// <summary>
        /// Checks if all the connected sources are on. If so, turns the door on.
        /// </summary>
        public void CheckPower()
        {
            OuterWildsSummerJam.LogInfo("Checking power...");

            foreach (PressurePlate source in powerSources)
            {
                if (!powered.Contains(source))
                {
                    if (RequireConstantPower) anim.SetBool("Open", false);
                    OuterWildsSummerJam.LogInfo($"Not all buttons are on! {powered.Count}/{powerSources.Count} active.");
                    return;
                }
            }

            OuterWildsSummerJam.LogSuccess("All buttons are on!");
            anim.SetBool("Open", true);
        }
    }
}