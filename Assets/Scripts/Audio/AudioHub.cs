using UnityEngine;

namespace Sound
{
    public class AudioHub : MonoBehaviour
    {
        private static AudioHub hub;

        [Header("Sound variables")]
        [SerializeField] private string interact; public static string Interact => hub.interact;
        [SerializeField] private string footstep; public static string Footstep => hub.footstep;
        [SerializeField] private string buttonclick; public static string ButtonClick => hub.buttonclick;
        [SerializeField] private string backgroundMusic; public static string BackgroundMusic => hub.backgroundMusic;
        [SerializeField] private string mirrorTurn; public static string MirrorTurn => hub.mirrorTurn;
        [SerializeField] private string inputSounds; public static string InputSound => hub.inputSounds;
        [SerializeField] private string bugDeath; public static string BugDeath => hub.bugDeath;
        [SerializeField] private string hit; public static string Hit => hub.hit;
        [SerializeField] private string sineWaveChange; public static string SineWaveChange => hub.sineWaveChange;
        [SerializeField] private string minigameSucces; public static string MinigameSucces => hub.minigameSucces;

        private void Start()
        {
            hub = this;
        }

        public static void PlaySound(string sound, GameObject go = null)
        {
            go = go == null ? hub.gameObject : go;
            AkSoundEngine.PostEvent(sound, go);
        }

        public static void SetState(string stategroup, string state) => AkSoundEngine.SetState(stategroup, state);

        public static void SetSwitch(string stategroup, string state, GameObject obj = null)
        {
            obj = obj == null ? hub.gameObject : obj;
            AkSoundEngine.SetSwitch(stategroup, state, obj);
        }
    }
}