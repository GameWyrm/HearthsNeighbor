using UnityEngine;

namespace HearthsNeighbor
{
    public class SetWaterVisibility : MonoBehaviour
    {
        public bool enableWater = true;

        private TessellatedSphereRenderer waterRenderer;

        private void Start()
        {
            waterRenderer = HearthsNeighbor.Main.LakePlanet.transform.Find("Sector/Water").GetComponent<TessellatedSphereRenderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                waterRenderer.enabled = enableWater;
            }
        }
    }
}
