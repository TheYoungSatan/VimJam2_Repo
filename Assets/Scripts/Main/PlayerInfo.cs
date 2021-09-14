using System;
using System.Collections;
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
        StartCoroutine(test());
    }

    private void Initialize()
    {
        AwakeTime = 0;
        HungerPercentage = 0;
        ThurstPercentage = 0;
        OnUpdateValues?.Invoke();
    }

    public void OnUpdate(int hunger = 20, int thurst = 20, int time = 5)
    {
        HungerPercentage += hunger;
        ThurstPercentage += thurst;
        AwakeTime += time;

        OnUpdateValues?.Invoke();
    }

    IEnumerator test()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            OnUpdate();
        }
    }
}
