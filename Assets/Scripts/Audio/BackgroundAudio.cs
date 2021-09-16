using Sound;
using System.Collections;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    [SerializeField] private string _state = "LivingRoom";
    [SerializeField] private string _footstepState = "LivingRoom";

    private IEnumerator Start()
    {
        AudioHub.SetState(AudioHub.BackgroundMusic, _state);
        AudioHub.SetSwitch(AudioHub.Footstep, _footstepState);
        yield return new WaitForEndOfFrame();
        if (!GameInfo.IsBackgroundSoundPlaying)
            AudioHub.PlaySound(AudioHub.BackgroundMusic);

        GameInfo.SetBackgroundSound(true);
    }
}