using System.Collections;
using UnityEngine;

namespace HearthsNeighbor
{
    public class ShipBarrierPanel : MonoBehaviour
    {
        public GameObject[] FirstPanels;
        public GameObject[] SecondPanels;
        public GameObject[] ThirdPanels;
        public GameObject[] FourthPanels;
        public GameObject[] FifthPanels;

        public GameObject Barrier;

        public AudioSource FailSound;
        public AudioSource SuccessSound;

        private GameObject[][] Panels;
        private Material barrierMat;
        private string code = "24125";
        private string inputCode = "";
        private bool disableBarrier = false;
        private float alpha = 0.7f;

        private void Start()
        {
            barrierMat = Barrier.GetComponent<MeshRenderer>().material;
            Panels = new GameObject[5][];
            for (int i = 0; i < Panels.Length; i++)
            {
                Panels[i] = new GameObject[5];
            }
            int index = 0;
            foreach (GameObject panel in FirstPanels)
            {
                Panels[0][index] = panel;
                index++;
            }
            index = 0;
            foreach (GameObject panel in SecondPanels)
            {
                Panels[1][index] = panel;
                index++;
            }
            index = 0;
            foreach (GameObject panel in ThirdPanels)
            {
                Panels[2][index] = panel;
                index++;
            }
            index = 0;
            foreach (GameObject panel in FourthPanels)
            {
                Panels[3][index] = panel;
                index++;
            }
            index = 0;
            foreach (GameObject panel in FifthPanels)
            {
                Panels[4][index] = panel;
                index++;
            }

            foreach (GameObject[] GoGroup in Panels)
            {
                foreach (GameObject go in GoGroup)
                {
                    go.SetActive(false);
                }
            }
        }

        private void Update()
        {
            if (disableBarrier && alpha > 0)
            {
                // Make the barrier progressively more transparent, and disable it once it's invisible.
                alpha -= Time.deltaTime / 3;
                if (alpha <= 0)
                {
                    Barrier.SetActive(false);
                }
                else
                {
                    barrierMat.color = new(0, 0, 1, alpha);
                }
            }
        }

        /// <summary>
        /// Activates a digit on the screen, and opens the blockade if the code is correct
        /// </summary>
        /// <param name="digit">Should be 0-4</param>
        public void InputDigit(int digit)
        {
            // if barrier is disabled, we don't need to run the panel code at all
            if (disableBarrier) return;

            // Show the proper digit on the panel
            Panels[inputCode.Length][digit].SetActive(true);

            inputCode += digit + 1;

            HearthsNeighbor.LogInfo($"Current Code:{inputCode}");

            if (inputCode.Length >= 5)
            {
                if (inputCode == code)
                {
                    SuccessSound.Play();
                    Barrier.GetComponent<Collider>().enabled = false;
                    disableBarrier = true;
                }
                else
                {
                    FailSound.Play();
                    inputCode = "";
                    foreach (GameObject[] GoGroup in Panels)
                    {
                        foreach (GameObject go in GoGroup)
                        {
                            go.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}