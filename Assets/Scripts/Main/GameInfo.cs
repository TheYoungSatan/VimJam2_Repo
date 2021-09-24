using Sound;
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
    public static bool IsBackgroundSoundPlaying = false;

    private PlayerInfo playerInfo;

    private void Awake()
    {
        instance = this;
        playerInfo = FindObjectOfType<PlayerInfo>();

        GameInfo[] objs = FindObjectsOfType<GameInfo>();
        if (objs.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        ResetInfo();
    }

    public static void ResetInfo()
    {
        FoodPieces = 0;
        UnpayedFood = 0;
        PouchMoney = 0;
        CurrentTime = 8;
        instance.playerInfo.Reset();
    }

    private void Update()
    {
        if(!playerInfo)
            playerInfo = FindObjectOfType<PlayerInfo>();
    }

    public static void ChangeFoodPiecesAmount(int amount) => FoodPieces += amount;  

    public static void ChangeUnpayedFoodPiecesAmount(int amount) => UnpayedFood += amount;

    public static void ChangePouchMoneyAmount(int amount) 
    { 
        PouchMoney += amount;
        if (PouchMoney <= 0)
            PouchMoney = 0;
    }

    public static void SetBackgroundSound(bool value) => IsBackgroundSoundPlaying = value;

    public static void AddTime(int time)
    {
        if (instance != null)
        {
            instance.playerInfo.AddAwakeTime(time);
        }
        
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

    public static void OnGameOver()
    {
        AudioHub.SetState(AudioHub.PlayerLife, "Dead");
        GUI.OnGameOver();
    }
}