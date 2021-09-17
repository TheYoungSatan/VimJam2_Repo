using Sound;
using UnityEngine;

namespace Wwise
{
    public class SwitchEventEnterCaller : AkTriggerBase
    {
        [SerializeField] private GameObject _checkObject;
        [SerializeField] private string _state = "LivingRoom";

        private void Awake()
        {
            if (!_checkObject)
                _checkObject = FindObjectOfType<PlayerController>().gameObject;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == _checkObject)
                AudioHub.SetSwitch(AudioHub.Footstep, _state);
        }
    }
}