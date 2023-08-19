using System.Collections;
using UnityEngine;

namespace HearthsNeighbor
{
    public class LavaHeatShader : MonoBehaviour
    {
        private GameObject lavaCore;
        private GameObject lavaObject;
        private Material myMat;

        private void Start()
        {
            lavaCore = HearthsNeighbor.Main.LavaPlanet;
            lavaObject = lavaCore.transform.Find("Sector/MoltenCore").gameObject;
            myMat = GetComponent<Renderer>().materials[0];
        }

        private void Update()
        {
            if (lavaCore != null && lavaObject != null)
            {
                myMat.SetVector("_BaseOrigin", lavaCore.transform.position);
                myMat.SetFloat("_LavaHeight", lavaObject.transform.localScale.x);
            }
        }
    }
}