using Sound;
using UnityEngine;

namespace Interacting
{
    public class Fridge : MonoBehaviour, IInteractable
    {
        [SerializeField] private int _decreaseValue = 85;
        private PlayerInfo _info;

        private void Awake()
        {
            _info = FindObjectOfType<PlayerInfo>();
        }

        public void OnInteract()
        {
            if(GameInfo.FoodPieces > 0)
            {
                _info.DecreaseHunger(_decreaseValue);
                AudioHub.PlaySound(AudioHub.Interact + "_Fridge");
                GameInfo.ChangeFoodPiecesAmount(-1);
            }
        }

        public bool Interactable()
        {
            return GameInfo.FoodPieces > 0 && _info.HungerPercentage > 0;
        }
    }
}