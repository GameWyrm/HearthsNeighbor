using UnityEngine;

namespace OuterWildsSummerJam
{
    public class SeamlessPlayerWarp : MonoBehaviour
    {
        public GameObject linkedWarp;

        private PlayerBody player;

        private void Awake()
        {
            OuterWildsSummerJam.RegisterWarp(transform.parent.gameObject);
        }

        private void Start()
        {
            player = (PlayerBody)Locator.GetPlayerBody();
            foreach (SeamlessPlayerWarp warp in FindObjectsOfType<SeamlessPlayerWarp>())
            {
                if (warp.gameObject != gameObject) linkedWarp = warp.gameObject;
            }
        }

        public void OnPressInteract()
        {
            TriggerWarp();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !OuterWildsSummerJam.Main.isInElevator)
            {
                OuterWildsSummerJam.Main.isInElevator = true;
                TriggerWarp();
            }
        }

        public void TriggerWarp()
        {
            if (linkedWarp == null)
            {
                OuterWildsSummerJam.LogError("No Linked Warp set or found!");
                return;
            }
            Vector3 difference = transform.position - player.transform.position;
            Vector3 targetPosition = linkedWarp.transform.position + difference;

            player.WarpToPositionRotation(targetPosition, linkedWarp.transform.rotation);
            OuterWildsSummerJam.LogInfo("Warped the player");
        }

    }
}
