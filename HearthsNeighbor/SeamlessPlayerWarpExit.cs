using UnityEngine;

namespace HearthsNeighbor
{
    public class SeamlessPlayerWarpExit : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && OuterWildsSummerJam.Main.isInElevator)
            {
                OuterWildsSummerJam.Main.isInElevator = false;
            }
        }
    }
}
