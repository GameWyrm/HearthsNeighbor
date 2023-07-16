using System.Collections;
using UnityEngine;

namespace HearthsNeighbor
{
    public class PlayEnding : MonoBehaviour
    {
        private AudioSource MusicPlayer;
        private bool hasActivated = false;

        private void Awake()
        {
            MusicPlayer = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !hasActivated)
            {
                HearthsNeighbor.LogInfo("Playing Ending music");
                MusicPlayer.Play();
                hasActivated = true;
            }
        }
    }
}