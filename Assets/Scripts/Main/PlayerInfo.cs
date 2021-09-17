using System;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public event Action OnUpdateValues;

    public Vector3 LivingRoomPlayerPos = Vector3.zero;

    public int AwakeTime { get; private set; }
    public int HungerPercentage { get; private set; }
    public int ThurstPercentage { get; private set; }
    public int WakeupTime => _wakeupTime;

    [SerializeField] private int _hungerPerHour = 10;
    [SerializeField] private int _thurstPerHour = 15;
    [SerializeField] private int _wakeupTime = 8;

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

    public void AddAwakeTime(int addAwaketime = 0)
    {
        AwakeTime += addAwaketime;
        SetValuesBasedOnTime(addAwaketime);
    }

    public void SetValuesBasedOnTime(int time)
    {
        HungerPercentage += _hungerPerHour * time; HungerPercentage = Mathf.Clamp(HungerPercentage, 0, 100);
        ThurstPercentage += _thurstPerHour * time; ThurstPercentage = Mathf.Clamp(ThurstPercentage, 0, 100);
        OnUpdateValues?.Invoke();
    }
}
