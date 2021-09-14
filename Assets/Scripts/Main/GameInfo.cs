using UnityEngine;

public class GameInfo : MonoBehaviour
{
    public static int FoodPieces = 0;

    private void Awake()
    {
        GameInfo[] objs = FindObjectsOfType<GameInfo>();
        if (objs.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
        FoodPieces = 5;
    }

    public static void ChangeFoodPiecesAmount(int amount) => FoodPieces += amount; 
}