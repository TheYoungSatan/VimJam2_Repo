using Sound;
using UnityEngine;
using Wwise;

[RequireComponent(typeof(SwitchEventCaller), typeof(AkState))]
public class BackgroundAudio : MonoBehaviour
{
    private SwitchEventCaller _caller;
    private void Start()
    {
        _caller = GetComponent<SwitchEventCaller>();
        _caller.CallSwitch();
        AudioHub.PlaySound(AudioHub.BackgroundMusic);
    }
}