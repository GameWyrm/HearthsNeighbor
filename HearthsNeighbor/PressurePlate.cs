using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HearthsNeighbor
{
    public class PressurePlate : MonoBehaviour
    {
        public PressurePlateDoor target;
        public Material[] buttonMats;
        public float offsetMultiplier = 1;
        public float scoutDistance = 2;
        public GameObject ExampleButton;
        public Light ExampleLight;

        private bool playerCollided = false;
        private bool scoutCollided = false;
        private bool allowColorChange = true;
        private Light buttonLight;
        private Renderer renderer;
        private GameObject player;
        private SurveyorProbe scout;
        private Collider collider;
        private AudioSource aud;
        private Shader correctShader;
        private Color endLightColor = Color.white;

        private void Start ()
        {
            target.RegisterPowerSource(this);
            renderer = GetComponent<Renderer>();
            collider = GetComponent<Collider>();
            player = Locator.GetPlayerBody().gameObject;
            scout = Locator.GetProbe();
            aud = GetComponent<AudioSource>();
            buttonLight = GetComponentInChildren<Light>();
            buttonLight.gameObject.SetActive(false);
            ColorUtility.TryParseHtmlString("#0FCC00", out endLightColor);
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
                        HearthsNeighbor.LogInfo($"Distance: {Vector3.Distance(checkPosition, scout.transform.position)}. Check Position: {checkPosition}. Scout Position: {scout.transform.position}");
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
                HearthsNeighbor.LogInfo("Player Collided!");
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
            if (!allowColorChange) return;
            if (scoutCollided || playerCollided)
            {
                //renderer.enabled = false;
                //glowButton.SetActive(true);
                ColorChange(1);
                target.PowerSource(this);
                aud.Play();
            }
            else
            {
                //renderer.enabled = true;
                //glowButton.SetActive(false);
                ColorChange(0);
                target.UnpowerSource(this);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Vector3 checkPosition = transform.position + transform.up;
            Gizmos.DrawWireSphere(checkPosition, scoutDistance);
        }

        /// <summary>
        /// Changes the color of the button
        /// 0: Default dim color
        /// 1: Bright color
        /// 2: Final color, prevents changing anymore
        /// </summary>
        /// <param name="slot"></param>
        public void ColorChange(int slot)
        {
            if (!allowColorChange) return;
            if (correctShader == null)
            {
                correctShader = renderer.sharedMaterial.shader;
            }

            renderer.sharedMaterial = buttonMats[slot];
            renderer.sharedMaterial.shader = correctShader;

            switch (slot)
            {
                case 0:
                    buttonLight.gameObject.SetActive(false);
                    break;
                case 1:
                    buttonLight.gameObject.SetActive(true);
                    break;
                case 2:
                    buttonLight.gameObject.SetActive(true);
                    buttonLight.color = endLightColor;
                    if (ExampleButton != null)
                    {
                        ExampleButton.GetComponent<Renderer>().material = renderer.sharedMaterial;
                        ExampleLight.color = endLightColor;
                    }

                    allowColorChange = false;
                    break;
            }
        }
    }
}