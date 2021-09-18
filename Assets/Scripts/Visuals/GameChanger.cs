using UnityEngine;
using UnityEngine.Tilemaps;

public class GameChanger : MonoBehaviour
{
    public static GameChanger instance;
    public VisualCompletionLevel CompletionLevel;
    private Tilemap _gameMap;

    private void Start()
    {
        instance = this;
        _gameMap = FindObjectOfType<Tilemap>();
        if (_gameMap)
            _gameMap.RefreshAllTiles();

    }
}