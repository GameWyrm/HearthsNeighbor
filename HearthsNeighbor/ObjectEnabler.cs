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
            Parent = GetComponentInParent<ObjectEnabler>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (Parent != null)
                {
                    ToggleObject(Parent.Target, EnableObject);
                }
                else
                {
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
