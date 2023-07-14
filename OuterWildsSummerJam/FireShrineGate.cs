using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OuterWildsSummerJam
{
    /// <summary>
    /// Script for the shrine blockade, requires all three buttons to open
    /// </summary>
    public class FireShrineGate : MonoBehaviour
    {
        public int buttonCount;
        public List<SpriteRenderer> buttonIndicators;
        public GameObject blockade;
        public Color dullColor;
        public Color brightColor;

        // Use this for initialization
        void Start()
        {
            foreach (var button in buttonIndicators)
            {
                button.color = dullColor;
            }
        }

        public void ActivateButton(int ID)
        {
            buttonIndicators[ID - 1].color = brightColor;
            buttonCount++;

            if (buttonCount >= 3)
            {
                blockade.SetActive(false);
            }
        }
    }
}