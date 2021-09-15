using UnityEngine;

namespace Interacting
{
    [RequireComponent(typeof(SwitchEventCaller), typeof(AkState))]
    public class Sink : MonoBehaviour, IInteractable
    {
        [SerializeField] private int _decreaseValue = 100;
        private PlayerInfo _info;
        private SwitchEventCaller _caller;

        private void Awake()
        {
            _info = FindObjectOfType<PlayerInfo>();

            if (TryGetComponent(out SwitchEventCaller caller))
                _caller = caller;
            else
                _caller = gameObject.AddComponent<SwitchEventCaller>();
        }

        public void OnInteract()
        {
            _info.DecreaseThurst(_decreaseValue);
            _caller.CallSwitch();
        }
    }
}
