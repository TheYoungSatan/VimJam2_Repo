using Sound;
using UnityEngine;

namespace Wwise
{
    public class SwitchEventExitCaller : AkTriggerBase
    {
        [SerializeField] private GameObject _checkObject;
        [SerializeField] private string _state = "Kitchen";

        private void Awake()
        {
            if (!_checkObject)
                _checkObject = FindObjectOfType<PlayerController>().gameObject;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject == _checkObject)
                AudioHub.SetSwitch(AudioHub.Footstep, _state);
        }
    }
}
