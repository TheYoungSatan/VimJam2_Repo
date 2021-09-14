﻿using MiniGame;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameHub : MonoBehaviour
{
    private enum GameStates { Running, GameOver, GameSucces}

    private enum GameChunk { Programming, Art, Sound, Publish}

    private enum Difficulty { Easy, Medium, Hard}

    [Serializable]
    private struct GameCollection
    {
        public GameChunk Chunk => _gameChunck;
        [SerializeField] private GameChunk _gameChunck;
        [SerializeField] private Minigame[] _minigames;

        public Minigame GetMinigame()
        {
            return _minigames[UnityEngine.Random.Range(0, _minigames.Length)];
        }
    }

    [SerializeField] private GameCollection[] _gameCollections;
    [SerializeField] private GameChunk _currentGameChunck;
    [SerializeField] private bool _testmode = true;
    [SerializeField] private Minigame _currentMiniGame;

    [Header("Scene loading")]
    [SerializeField] private string _returnScene;

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

    public void SetGameChunk()
    {

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

    public void OnReturn()
    {
        SceneManager.LoadScene(_returnScene);
    }
}
