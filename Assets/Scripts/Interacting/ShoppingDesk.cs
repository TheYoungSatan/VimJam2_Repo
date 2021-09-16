using UnityEngine;

namespace Interacting
{
    public class ShoppingDesk : MonoBehaviour, IInteractable
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private SpriteRenderer _shopBox;

        private bool _hasFood = true;

        private void Awake()
        {
            int r = UnityEngine.Random.Range(0, _sprites.Length);
            _shopBox.sprite = _sprites[r];
            _hasFood = r == 0 ? false : true;
        }

        public bool Interactable()
        {
            return _hasFood && GameInfo.PouchMoney > GameInfo.FoodCost;
        }

        public void OnInteract()
        {
            if (!_hasFood || GameInfo.PouchMoney < GameInfo.FoodCost) return;

            GameInfo.ChangeUnpayedFoodPiecesAmount(1);
            _hasFood = false;
            _shopBox.sprite = _sprites[0];
        }
    }
}
