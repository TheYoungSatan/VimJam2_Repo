using Sound;
using UnityEngine;

namespace Interacting
{
    public class Fridge : MonoBehaviour, IInteractable
    {
        [SerializeField] private int _decreaseValue = 85;
        private PlayerInfo _info;
        private GUI _gui;

        private void Awake()
        {
            _info = FindObjectOfType<PlayerInfo>();
            _gui = FindObjectOfType<GUI>();
        }

        public void OnInteract()
        {
            if(GameInfo.FoodPieces > 0)
            {
                _info.DecreaseHunger(_decreaseValue);
                GameInfo.ChangeFoodPiecesAmount(-1);
                AudioHub.PlaySound(AudioHub.Interact + "_Fridge");
                _gui.UpdateGUI();
            }
        }

        public bool Interactable()
        {
            return GameInfo.FoodPieces > 0 && _info.HungerPercentage > 0;
        }

        public string InfoText()
        {
            return $"Fridge \n Food: {GameInfo.FoodPieces}";
        }

        public bool HasInfoPanel()
        {
            return true;
        }

        public Transform Position()
        {
            return transform;
        }
    }
}