using Sound;
using UnityEngine;

namespace Interacting
{
    public class ShoppingDesk : MonoBehaviour, IInteractable
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private SpriteRenderer _shopBox;

        private bool _hasFood = true;
        int r;

        private void Awake()
        {
            r = UnityEngine.Random.Range(0, _sprites.Length);
            _shopBox.sprite = _sprites[r];
            _hasFood = r == 0 ? false : true;
        }

        public bool Interactable()
        {
            return (_hasFood && GameInfo.PouchMoney > GameInfo.FoodCost) || (!_hasFood && GameInfo.UnpayedFood > 0);
        }

        public void OnInteract()
        {
            if (!_hasFood) 
            { 
                if(GameInfo.UnpayedFood > 0)
                {
                    r = r == 0 ? UnityEngine.Random.Range(1, _sprites.Length) : r;
                    _shopBox.sprite = _sprites[r];
                    GameInfo.ChangeUnpayedFoodPiecesAmount(-1);
                    _hasFood = true;
                }
                return; 
            }

            if (GameInfo.PouchMoney < GameInfo.FoodCost)
                return;

            GameInfo.ChangeUnpayedFoodPiecesAmount(1);
            _hasFood = false;
            _shopBox.sprite = _sprites[0];
            AudioHub.PlaySound(AudioHub.Interact + "_shoppingDesk");
        }

        public string InfoText()
        {
            return "";
        }

        public bool HasInfoPanel()
        {
            return false;
        }

        public Transform Position()
        {
            return transform;
        }
    }
}
