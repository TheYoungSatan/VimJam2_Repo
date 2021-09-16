using System;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
    private static GameInfo instance;

    public static int FoodPieces = 0;
    public static int UnpayedFood = 0;
    public static int PouchMoney = 12;
    public static int FoodCost = 5;
    public static int CurrentTime = 10;
    public static int TravelTime = 1;

    private PlayerInfo playerInfo;

    private void Awake()
    {
        instance = this;
        playerInfo = FindObjectOfType<PlayerInfo>();

        GameInfo[] objs = FindObjectsOfType<GameInfo>();
        if (objs.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public static void OnSceneSwitch()
    {
        AddTime(2);
        instance.playerInfo.AddAwakeTime(2);
    }

    public static void ChangeFoodPiecesAmount(int amount) { Debug.Log($"Change food: \nResult = {FoodPieces} -> {FoodPieces + amount}"); FoodPieces += amount;  }

    public static void ChangeUnpayedFoodPiecesAmount(int amount) => UnpayedFood += amount;

    public static void ChangePouchMoneyAmount(int amount) 
    { 
        PouchMoney += amount;
        if (PouchMoney <= 0)
            PouchMoney = 0;
    }

    public static void AddTime(int time)
    {
        CurrentTime += time;
        if (CurrentTime > 24) 
            CurrentTime = CurrentTime - 24;
    }

    public static void SetTime(int time)
    {
        int timepast = (24 - CurrentTime) + time;
        instance.playerInfo.SetValuesBasedOnTime(timepast);
        instance.playerInfo.ResetAwaketime();
    }
}