using UnityEngine;

public class GameInfo : MonoBehaviour
{
    public static int FoodPieces = 0;
    public static int UnpayedFood = 0;
    public static int PouchMoney = 12;
    public static int FoodCost = 5;

    private void Awake()
    {
        GameInfo[] objs = FindObjectsOfType<GameInfo>();
        if (objs.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public static void ChangeFoodPiecesAmount(int amount) => FoodPieces += amount;

    public static void ChangeUnpayedFoodPiecesAmount(int amount) => UnpayedFood += amount;

    public static void ChangePouchMoneyAmount(int amount) 
    { 
        PouchMoney += amount;
        if (PouchMoney <= 0)
            PouchMoney = 0;
    }
}