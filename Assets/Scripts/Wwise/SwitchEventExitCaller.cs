using UnityEngine;

namespace Wwise
{
    public class SwitchEventExitCaller : AkTriggerBase
    {
        [SerializeField] private GameObject _checkObject;

        private void Awake()
        {
            if (!_checkObject)
                _checkObject = FindObjectOfType<PlayerController>().gameObject;
        }

        private void Update()
        {
            if (!_checkObject)
                _checkObject = FindObjectOfType<PlayerController>().gameObject;
        }

        public void CallSwitch()
        {
            if (triggerDelegate != null)
            {
                triggerDelegate(_checkObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log("Exit");
            if (collision.gameObject == _checkObject)
                CallSwitch();
        }
    }
}
