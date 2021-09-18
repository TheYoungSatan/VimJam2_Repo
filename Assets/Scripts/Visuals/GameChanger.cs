using UnityEngine;
using UnityEngine.Tilemaps;

public enum CompletionLevel { Easy, Medium, Hard, Failed }

public class GameChanger : MonoBehaviour
{
    public static GameChanger instance;

    public CompletionLevel EnvironmentCompletionLevel;
    public CompletionLevel PlayerVisualCompletionLevel;
    public CompletionLevel MovementCompletionLevel;
    public CompletionLevel AnimationCompletionLevel;

    private Tilemap _gameMap;

    private void Start()
    {
        instance = this;
        _gameMap = FindObjectOfType<Tilemap>();
        if (_gameMap)
            _gameMap.RefreshAllTiles();

    }
}