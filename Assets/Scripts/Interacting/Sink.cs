using Sound;
using UnityEngine;

namespace Interacting
{
    public class Sink : MonoBehaviour, IInteractable
    {
        [SerializeField] private int _decreaseValue = 100;
        private PlayerInfo _info;

        private void Awake()
        {
            _info = FindObjectOfType<PlayerInfo>();
        }

        public void OnInteract()
        {
            _info.DecreaseThurst(_decreaseValue);
            AudioHub.PlaySound(AudioHub.Interact + "_Sink");
        }

        public bool Interactable()
        {
            return _info.ThurstPercentage > 0;
        }
    }
}
