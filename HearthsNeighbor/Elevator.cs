using System.Collections;
using UnityEngine;

namespace HearthsNeighbor
{
    public class Elevator : MonoBehaviour
    {
        [Tooltip("Determines if the elevator goes down")]
        public bool OnMainPlanet;
        [Tooltip("Name of the elevator that this elevator connects to")]
        public string TargetElevatorName;

        private Animator anim;
        private AnimationEvent evt;
        private Elevator linkedElevator;
        private PlayerBody player;

        private void Awake()
        {
            player = (PlayerBody)Locator.GetPlayerBody();

            foreach (Elevator elvt in Resources.FindObjectsOfTypeAll<Elevator>())
            {
                if (elvt.gameObject.name == TargetElevatorName)
                {
                    linkedElevator = elvt;
                    break;
                }
            }

            if (OnMainPlanet)
            {
                AnimationClip mainClip = null;
                foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
                {
                    if (clip.length > 0.5f)
                    {
                        mainClip = clip;
                        break;
                    }
                }
                if (mainClip == null)
                {
                    HearthsNeighbor.LogError("Could not find an appropriate clip for the elevator!");
                    return;
                }
                evt.time = mainClip.length;
                evt.functionName = "Transport";
                mainClip.AddEvent(evt);
            }
        }

        public void StartTransport()
        {
            anim.SetBool("IsDescending", OnMainPlanet);
        }

        public void Transport()
        {

            Vector3 difference = transform.InverseTransformPoint(player.GetPosition());
            Quaternion angleDifference = transform.InverseTransformRotation(player.GetRotation());
            Vector3 targetPosition = linkedElevator.transform.TransformPoint(difference);
            Quaternion targetRotation = linkedElevator.transform.rotation * angleDifference;

            OWRigidbody planet = linkedElevator.GetAttachedOWRigidbody();

            player.WarpToPositionRotation(targetPosition, targetRotation);
            player.SetVelocity(planet.GetPointVelocity(player.GetPosition()));
        }
    }
}