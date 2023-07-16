using UnityEngine;

namespace HearthsNeighbor
{
    public class ObjectEnabler : MonoBehaviour
    {
        /// <summary>
        /// Set this if it's paired with another enabler to instead use the parent's 
        /// </summary>
        public ObjectEnabler Parent;
        /// <summary>
        /// If false, will disable the object instead
        /// </summary>
        public bool EnableObject = true;
        /// <summary>
        /// Object that gets enabled or disabled
        /// </summary>
        public GameObject Target;

        private void Start()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (Parent != null)
                {
                    HearthsNeighbor.LogInfo($"{gameObject.name} is setting the active value of its parent script, {Parent.Target.gameObject.name}, to {EnableObject}.");
                    ToggleObject(Parent.Target, EnableObject);
                }
                else
                {
                    HearthsNeighbor.LogInfo($"{gameObject.name} is setting the active value of {Target.gameObject.name} to {EnableObject}.");
                    ToggleObject(Target, EnableObject);
                }
            }
        }

        private void ToggleObject(GameObject obj, bool shouldEnable)
        {

            obj.SetActive(shouldEnable);
        }
    }
}
