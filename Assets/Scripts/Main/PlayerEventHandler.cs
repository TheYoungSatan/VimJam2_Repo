using Sound;
using UnityEngine;

public class PlayerEventHandler : MonoBehaviour
{
    public void OnFootStep()
    {
        AudioHub.PlaySound(AudioHub.Footstep);
    }
}