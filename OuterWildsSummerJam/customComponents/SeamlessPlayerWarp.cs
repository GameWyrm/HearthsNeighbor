using UnityEngine;

namespace OuterWildsSummerJam.customComponents
{
    public class SeamlessPlayerWarp : MonoBehaviour
    {
        public GameObject linkedWarp;

        private PlayerBody player;

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
