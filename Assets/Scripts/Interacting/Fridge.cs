using UnityEngine;

namespace Interacting
{
    [RequireComponent(typeof(SwitchEventCaller), typeof(AkState))]
    public class Fridge : MonoBehaviour, IInteractable
    {
        [SerializeField] private int _decreaseValue = 85;
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
            if(GameInfo.FoodPieces > 0)
            {
                _info.DecreaseHunger(_decreaseValue);
                _caller.CallSwitch();
                GameInfo.ChangeFoodPiecesAmount(-1);
            }
        }
    }
}