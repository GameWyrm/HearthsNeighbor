using UnityEngine;

namespace HearthsNeighbor
{
    public class CustomInteraction : MonoBehaviour
    {
        public float interactCheckDistance;

        private GameObject player;
        private Collider col;
        private SingleInteractionVolume _interactVolume;

        private void Awake()
        {
            _interactVolume = this.GetRequiredComponent<SingleInteractionVolume>();
            _interactVolume.OnPressInteract += OnPressInteract;
            GlobalMessenger.AddListener("SuitUp", new Callback(OnSuitUp));
            GlobalMessenger.AddListener("RemoveSuit", new Callback(OnRemoveSuit));
        }

        private void Start()
        {
            player = Locator.GetPlayerBody().gameObject;
            col = GetComponent<Collider>();
            _interactVolume.DisableInteraction();
            enabled = false;
            _interactVolume._textID = UITextType.HoldPrompt;
        }
        private void OnSuitUp()
        {
            _interactVolume.EnableInteraction();
        }

        private void OnRemoveSuit()
        {
            _interactVolume.DisableInteraction();
        }

        private void Update()
        {

            if (col.enabled == false)
            {
                if (Vector3.Distance(transform.position, player.transform.position) < interactCheckDistance)
                {
                    //GetComponent<Collider>().enabled = true;
                    HearthsNeighbor.LogInfo("Interactable Enabled!");
                }
            }
        }

        private void OnPressInteract()
        {
            HearthsNeighbor.LogSuccess("You interacted with me!");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, interactCheckDistance);
        }
    }
}
