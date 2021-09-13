using MiniGame;
using System;
using System.Linq;
using UnityEngine;

public class MinigameHub : MonoBehaviour
{
    private enum GameStates { Running, GameOver, GameSucces}

    private enum GameChunck { Programming, Art, Sound, Publish}

    private enum Difficulty { Easy, Medium, Hard}

    [Serializable]
    private struct GameCollection
    {
        public GameChunck Chunk => _gameChunck;
        [SerializeField] private GameChunck _gameChunck;
        [SerializeField] private Minigame[] _minigames;

        public Minigame GetMinigame()
        {
            return _minigames[UnityEngine.Random.Range(0, _minigames.Length)];
        }
    }

    [SerializeField] private GameCollection[] _gameCollections;
    [SerializeField] private GameChunck _currentGameChunck;
    [SerializeField] private bool _testmode = true;
    [SerializeField] private Minigame _currentMiniGame;

    private Minigame _playedGame;
    private GameStates _currentState = GameStates.Running;

    void Start()
    {
        if (!_testmode)
        {
            GameCollection collection = _gameCollections.Aggregate((p, n) => p.Chunk == _currentGameChunck ? p : n);
            _currentMiniGame = collection.GetMinigame();
        }

        InitializeGame();
    }

    private void InitializeGame()
    {
        if (_currentMiniGame != null)
        {
            Minigame game = Instantiate(_currentMiniGame.gameObject).GetComponent<Minigame>();
            game.Hub = this;
            game.RunGame();
            _playedGame = game;
        }
    }

    private void Update()
    {
        if (_currentState == GameStates.Running) 
        {
            _playedGame?.CheckInput();
            _playedGame?.UpdateGame();
        }
    }

    public void OnGameOver()
    {
        _currentState = GameStates.GameOver;
        Debug.Log("GameOver");
    }

    public void OnGameSucces()
    {
        _currentState = GameStates.GameSucces;
        Debug.Log("Succes");
    }
}
