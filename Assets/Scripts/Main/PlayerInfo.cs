using System;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public event Action OnUpdateValues;

    public int AwakeTime { get; private set; }
    public int HungerPercentage { get; private set; }
    public int ThurstPercentage { get; private set; }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        AwakeTime = 0;
        HungerPercentage = 0;
        ThurstPercentage = 0;
        OnUpdateValues?.Invoke();
    }

    public void DecreaseHunger(int percentage) 
    { 
        HungerPercentage -= percentage; HungerPercentage = Mathf.Clamp(HungerPercentage, 0, 100);
        OnUpdateValues?.Invoke();
    }
    public void DecreaseThurst(int percentage) 
    { 
        ThurstPercentage -= percentage; ThurstPercentage = Mathf.Clamp(ThurstPercentage, 0, 100);
        OnUpdateValues?.Invoke();
    }
    public void ResetAwaketime()
    {
        AwakeTime = 0;
        OnUpdateValues?.Invoke();
    }

    public void OnUpdateStats(int addhunger = 20, int addthurst = 20, int addAwaketime = 5)
    {
        HungerPercentage += addhunger; HungerPercentage = Mathf.Clamp(HungerPercentage, 0, 100);
        ThurstPercentage += addthurst; ThurstPercentage = Mathf.Clamp(ThurstPercentage, 0, 100);
        AwakeTime += addAwaketime;

        OnUpdateValues?.Invoke();
    }
}
