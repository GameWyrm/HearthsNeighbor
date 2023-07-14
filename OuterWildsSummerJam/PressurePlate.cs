using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OuterWildsSummerJam
{
    public class PressurePlate : MonoBehaviour
    {
        public PressurePlateDoor target;
        public Renderer glowRenderer;
        public float scoutDistance = 1;
        public float offsetMultiplier = 1;

        private bool playerCollided = false;
        private bool scoutCollided = false;
        private Renderer renderer;
        private GameObject player;
        private SurveyorProbe scout;
        private Collider collider;

        private void Start ()
        {
            target.RegisterPowerSource(this);
            renderer = GetComponent<Renderer>();
            collider = GetComponent<Collider>();
            player = Locator.GetPlayerBody().gameObject;
            scout = Locator.GetProbe();
            glowRenderer.enabled = false;
        }

        private void Update ()
        {
            if (scout.IsLaunched())
            {
                // I really hate this but we need to disable the collider or the scout will just shoot through the plate instead of hitting it.
                if (!scout.IsAnchored() && Vector3.Distance(transform.position, scout.transform.position) < 5f)
                {
                    collider.enabled = false;
                }
                else
                {
                    collider.enabled = true;
                }

                Vector3 checkPosition = transform.position + transform.up;
                if (Vector3.Distance(checkPosition, scout.transform.position) < scoutDistance)
                {
                    if (!scoutCollided)
                    {
                        scoutCollided = true;
                        CheckColliders();
                        OuterWildsSummerJam.LogInfo($"Distance: {Vector3.Distance(checkPosition, scout.transform.position)}. Check Position: {checkPosition}. Scout Position: {scout.transform.position}");
                    }
                }
                else if (scoutCollided)
                {
                    scoutCollided = false;
                    CheckColliders();
                }
            }
            if (!scout.IsLaunched() && scoutCollided)
            {
                scoutCollided = false;
                CheckColliders();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerCollided = true;
                CheckColliders();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerCollided = false;
                CheckColliders();
            }
        }

        /// <summary>
        /// Determines if the plate has at least one valid connection for power
        /// </summary>
        private void CheckColliders()
        {
            if (scoutCollided || playerCollided)
            {
                renderer.enabled = false;
                glowRenderer.enabled = true;
                target.PowerSource(this);
            }
            else
            {
                renderer.enabled = true;
                glowRenderer.enabled = false;
                target.UnpowerSource(this);
            }
        }
    }
}