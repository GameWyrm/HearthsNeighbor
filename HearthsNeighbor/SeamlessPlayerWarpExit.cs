using UnityEngine;

namespace HearthsNeighbor
{
    public class SeamlessPlayerWarpExit : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && HearthsNeighbor.Main.isInElevator)
            {
                HearthsNeighbor.Main.isInElevator = false;
            }
        }
    }
}
