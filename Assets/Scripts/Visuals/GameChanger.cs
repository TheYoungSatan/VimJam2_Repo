using UnityEngine;
using UnityEngine.Tilemaps;

public enum CompletionLevel { Easy = 1, Medium = 2, Hard = 3, Failed = 0 }

public class GameChanger : MonoBehaviour
{
    public static GameChanger instance;

    public CompletionLevel EnvironmentCompletionLevel;
    public CompletionLevel PlayerVisualCompletionLevel;
    public CompletionLevel MovementCompletionLevel;

    private Tilemap _gameMap;

    private void Awake()
    {
        instance = this;
        _gameMap = FindObjectOfType<Tilemap>();
        if (_gameMap)
            _gameMap.RefreshAllTiles();

    }
}