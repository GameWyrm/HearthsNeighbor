using System.Collections;
using UnityEngine;

namespace OuterWildsSummerJam
{
    public class SeamlessPlayerWarp : MonoBehaviour
    {
        public GameObject linkedWarp;
        public GameObject myPlanet;
        public Animator anim;
        public bool goDown = false;

        private PlayerBody player;
        private bool speedUpCrystals;
        private bool slowDownCrystals;
        private float crystalTime;
        private Animator linkedAnim;
        private SurveyorProbe scout;
        private ShipHUDMarker shipMarker;

        private void Awake()
        {
            OuterWildsSummerJam.RegisterWarp(transform.parent.transform.parent.gameObject);
            myPlanet = this.GetAttachedOWRigidbody().gameObject;
        }

        private void Start()
        {
            player = (PlayerBody)Locator.GetPlayerBody();
            scout = Locator.GetProbe();
            shipMarker = FindObjectOfType<ShipHUDMarker>();
        }

        private void Update()
        {
            if (speedUpCrystals || slowDownCrystals)
            {
                float targetSpeed = Mathf.Lerp(0, 10, crystalTime / 3);
                if (speedUpCrystals )
                {
                    crystalTime += Time.deltaTime;
                }
                else if (slowDownCrystals )
                {
                    crystalTime -= Time.deltaTime;
                }
                if (crystalTime >= 3)
                {
                    crystalTime = 3;
                    speedUpCrystals = false;
                    //targetSpeed = 3;
                    OuterWildsSummerJam.LogInfo("Disabling speedup");
                }
                if (crystalTime < 0)
                {
                    crystalTime = 0;
                    slowDownCrystals = false;
                    targetSpeed = 0;
                    OuterWildsSummerJam.LogInfo("Disabling slowdown");
                }
                anim.SetFloat("CrystalSpeed", targetSpeed * (goDown ? -1 : 1));
                linkedAnim.SetFloat("CrystalSpeed", targetSpeed * (goDown ? -1 : 1));
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

                linkedAnim = linkedWarp.GetComponent<SeamlessPlayerWarp>().anim;

                StartCoroutine(TriggerWarp());
            }
        }

        public IEnumerator TriggerWarp()
        {
            if (goDown)
            {
                GameObject body = linkedWarp.GetComponent<SeamlessPlayerWarp>().myPlanet;
                foreach (Transform t in body.transform)
                {
                    t.gameObject.SetActive(true);
                }
            }
            anim.SetTrigger("CloseDoor");
            linkedAnim.SetTrigger("CloseDoor");
            yield return new WaitForSeconds(3);

            Vector3 difference = transform.InverseTransformPoint(player.GetPosition());
            Quaternion angleDifference = transform.InverseTransformRotation(player.GetRotation());
            Vector3 targetPosition = linkedWarp.transform.TransformPoint(difference);
            Quaternion targetRotation = linkedWarp.transform.rotation * angleDifference;

            OWRigidbody planet = linkedWarp.GetAttachedOWRigidbody();

            player.WarpToPositionRotation(targetPosition, targetRotation);
            player.SetVelocity(planet.GetPointVelocity(player.GetPosition()));

            scout.ExternalRetrieve(false);
            if (goDown)
            {
                shipMarker._isVisible = false;
            }

            OuterWildsSummerJam.LogInfo("Warped the player");

            speedUpCrystals = true;

            yield return new WaitForSeconds(7);

            OuterWildsSummerJam.LogInfo("Waited");

            slowDownCrystals = true;

            yield return new WaitForSeconds(5);

            OuterWildsSummerJam.LogInfo("WaitedMore");
            anim.SetTrigger("OpenDoor");
            linkedAnim.SetTrigger("OpenDoor");
            yield return new WaitForSeconds(1);
            if (!goDown)
            {
                foreach (Transform t in myPlanet.transform)
                {
                    t.gameObject.SetActive(false);
                }
            }
        }
    }
}
