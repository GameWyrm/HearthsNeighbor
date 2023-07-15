using System.Collections;
using UnityEngine;

namespace HearthsNeighbor
{
    public class DisplayBounds : MonoBehaviour
    {
        public float[] radii;

        public Color[] colors;

        private void OnDrawGizmos()
        {
            int index = 0;
            foreach (float radius in radii)
            {
                if (index < colors.Length)
                {
                    Gizmos.color = colors[index];
                }
                else Gizmos.color = Color.cyan;

                Gizmos.DrawWireSphere(transform.position, radius);
                index++;
            }
        }
        
    }
}