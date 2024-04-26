using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HearthsNeighbor
{
    public class MagistariumConnection : MonoBehaviour
    {
        private bool hasBeenTalkedTo = false;
        private SingleInteractionVolume _interactVolume;

        private void Start()
        {
            _interactVolume = GetComponent<SingleInteractionVolume>();
            _interactVolume.OnPressInteract += EndingSequence;
        }

        private void EndingSequence()
        {
            if (!hasBeenTalkedTo)
            {
                GlobalMessenger.AddListener("ExitConversation", OnRead);
                hasBeenTalkedTo = true;
            }
        }

        private void OnRead()
        {
            GlobalMessenger.RemoveListener("ExitConversation", OnRead);
            StartCoroutine(EndingNotifications());
        }

        private IEnumerator EndingNotifications()
        {
            yield return new WaitForSeconds(10);
            NotificationManager.SharedInstance.PostNotification(new(TranslateString("$HN2Connection1")));
            yield return new WaitForSeconds(5);
            NotificationManager.SharedInstance.PostNotification(new(TranslateString("$HN2Connection2")));
            yield return new WaitForSeconds(5);
            NotificationManager.SharedInstance.PostNotification(new(TranslateString("$HN2Connection3")));
            yield return new WaitForSeconds(5);
            NotificationManager.SharedInstance.PostNotification(new(TranslateString("$HN2Connection4")));
            Locator.GetShipLogManager().RevealFact("HN_POD_HN2");
        }

        private string TranslateString(string str)
        {
            return HearthsNeighbor.Main.nh.GetTranslationForOtherText(str);
        }
    }
}
