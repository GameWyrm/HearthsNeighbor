using UnityEngine;

namespace HearthsNeighbor
{
    public class FaceAnotherObject : MonoBehaviour
    {
        /// <summary>
        /// Body that the target is on
        /// </summary>
        public JamBody body;
        /// <summary>
        /// Path to the object this object will look at, should not include the body
        /// Does not need to be set if targetObject is set
        /// </summary>
        public string targetPath;
        /// <summary>
        /// The object this object will look at
        /// </summary>
        public GameObject targetObject;

        private void Start()
        {
            if (targetObject == null)
            {
                GameObject targetBody = null;
                switch (body)
                {
                    case JamBody.MainPlanet:
                        targetBody = HearthsNeighbor.Main.MainPlanet;
                        break;
                    case JamBody.AlpinePlanet:
                        targetBody = HearthsNeighbor.Main.AlpinePlanet;
                        break;
                    case JamBody.LakePlanet:
                        targetBody = HearthsNeighbor.Main.LakePlanet;
                        break;
                    case JamBody.LavaPlanet:
                        targetBody = HearthsNeighbor.Main.LavaPlanet;
                        break;
                    case JamBody.DerelictShip:
                        targetBody = HearthsNeighbor.Main.DerelictShip;
                        break;
                }
                if (targetBody != null)
                {
                    targetObject = targetBody.transform.Find(targetPath).gameObject;
                }

                if (targetObject == null) HearthsNeighbor.LogError($"Target for {gameObject.name} was not found at path {targetBody}/{targetPath}!");
            }
            if (targetObject != null) HearthsNeighbor.LogSuccess($"Object {gameObject.name} found its target {targetObject.name}!");
        }

        private void Update()
        {
            if (targetObject != null)
            {
                transform.LookAt(targetObject.transform.position);
            }
        }

        public enum JamBody
        {
            MainPlanet,
            AlpinePlanet,
            LakePlanet,
            LavaPlanet,
            DerelictShip
        }
    }
}
