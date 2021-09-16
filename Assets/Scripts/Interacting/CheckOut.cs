using System;
using UnityEngine;

namespace Interacting
{
    public class CheckOut : MonoBehaviour, IInteractable
    {
        [SerializeField] private int _costFoodPiece = GameInfo.FoodCost;
        [SerializeField] private GUI _gui;

        private void Awake()
        {
            _gui = FindObjectOfType<GUI>();
        }

        public bool Interactable()
        {
            return GameInfo.UnpayedFood > 0;
        }

        public void OnInteract()
        {
            if (GameInfo.UnpayedFood <= 0) return;

            if (GameInfo.PouchMoney < _costFoodPiece)
            {
                GameInfo.ChangeUnpayedFoodPiecesAmount(-GameInfo.UnpayedFood);
                return;
            }

            int cost = GameInfo.FoodCost * GameInfo.UnpayedFood;
            int affordable = CheckAffordable();
            cost = cost > GameInfo.PouchMoney ? GameInfo.FoodCost * affordable : cost;
            GameInfo.ChangePouchMoneyAmount(-cost);
            GameInfo.ChangeFoodPiecesAmount(cost / affordable);
            GameInfo.ChangeUnpayedFoodPiecesAmount(-GameInfo.UnpayedFood);

            _gui.UpdateGUI();
        }

        private int CheckAffordable()
        {
            int rv = 0;
            for (int i = 1; i <= GameInfo.UnpayedFood; i++)
            {
                if (GameInfo.PouchMoney > GameInfo.FoodCost * i)
                    rv++;
                else
                    break;
            }
            return rv;
        }
    }
}
