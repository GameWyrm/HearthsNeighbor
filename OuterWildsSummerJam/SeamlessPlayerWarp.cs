using System.Collections;
using UnityEngine;

namespace OuterWildsSummerJam
{
    public class SeamlessPlayerWarp : MonoBehaviour
    {
        public GameObject linkedWarp;
        public Animator anim;
        public bool goDown = false;

        private PlayerBody player;
        private bool speedUpCrystals;
        private bool slowDownCrystals;
        private float crystalTime;

        private void Awake()
        {
            OuterWildsSummerJam.RegisterWarp(transform.parent.transform.parent.gameObject);
        }

        private void Start()
        {
            player = (PlayerBody)Locator.GetPlayerBody();
            foreach (SeamlessPlayerWarp warp in FindObjectsOfType<SeamlessPlayerWarp>())
            {
                if (warp.gameObject != gameObject) linkedWarp = warp.gameObject;
            }
        }

        private void Update()
        {
            if (speedUpCrystals || slowDownCrystals)
            {
                float targetSpeed = Mathf.Lerp(0, 10, crystalTime);
                anim.SetFloat("CrystalSpeed", targetSpeed);
                if (speedUpCrystals )
                {
                    crystalTime += (Time.deltaTime / 3) * (goDown ? 1 : -1);
                }
                else if (slowDownCrystals )
                {
                    crystalTime -= (Time.deltaTime / 3) * (goDown ? 1 : -1);
                }
                if (crystalTime >= 3)
                {
                    crystalTime = 3;
                    speedUpCrystals = false;
                }
                if (crystalTime < 0)
                {
                    crystalTime = 0;
                    slowDownCrystals = false;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player") && !OuterWildsSummerJam.Main.isInElevator)
            {
                OuterWildsSummerJam.Main.isInElevator = true;
                if (linkedWarp == null)
                {
                    OuterWildsSummerJam.LogError("No Linked Warp set or found!");
                    return;
                }
                StartCoroutine(TriggerWarp());
            }
        }

        public IEnumerator TriggerWarp()
        {
            anim.SetTrigger("CloseDoor");
            yield return new WaitForSeconds(3);

            Vector3 difference = transform.InverseTransformPoint(player.GetPosition());
            Quaternion angleDifference = transform.InverseTransformRotation(player.GetRotation());
            Vector3 targetPosition = linkedWarp.transform.TransformPoint(difference);
            Quaternion targetRotation = linkedWarp.transform.rotation * angleDifference;

            OWRigidbody planet = linkedWarp.GetAttachedOWRigidbody();

            player.WarpToPositionRotation(targetPosition, targetRotation);
            player.SetVelocity(planet.GetPointVelocity(player.GetPosition()));
            OuterWildsSummerJam.LogInfo("Warped the player");

            speedUpCrystals = true;

            yield return new WaitForSeconds(7);

            slowDownCrystals = true;

            yield return new WaitForSeconds(5);

            anim.SetTrigger("OpenDoor");
        }
    }
}
