using UnityEngine;

namespace HearthsNeighbor
{
    public class CopyRuleset : MonoBehaviour
    {
        private void Start ()
        {
            var myRuleset = GetComponent<EffectRuleset>();
            var copyRuleset = HearthsNeighbor.Main.LakePlanet.transform.Find("Volumes/Ruleset").GetComponent<EffectRuleset>();


        }
    }
}
