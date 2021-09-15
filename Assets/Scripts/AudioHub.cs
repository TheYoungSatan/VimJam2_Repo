﻿using UnityEngine;

namespace Sound
{
    public class AudioHub : MonoBehaviour
    {
        private static AudioHub hub;

        [Header("Sound variables")]
        [SerializeField] private string interact; public static string Interact => hub.interact;
        [SerializeField] private string footstep; public static string Footstep => hub.footstep;
        [SerializeField] private string buttonclick; public static string ButtonClick => hub.buttonclick;

        private void Start()
        {
            hub = this;
        }

        public static void PlaySound(string sound, GameObject go = null)
        {
            go = go == null ? hub.gameObject : go;
            AkSoundEngine.PostEvent(sound, go);
        }
    }
}