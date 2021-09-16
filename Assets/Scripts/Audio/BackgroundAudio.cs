using Sound;
using UnityEngine;
using Wwise;

public class BackgroundAudio : MonoBehaviour
{
    [SerializeField] private string _state = "LivingRoom";
    private void Start()
    {
        AudioHub.PlaySound(AudioHub.BackgroundMusic);
        AudioHub.SetState(AudioHub.BackgroundMusic, _state);
    }
}