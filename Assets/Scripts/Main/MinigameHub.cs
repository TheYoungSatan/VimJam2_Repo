using MiniGame;
using Sound;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameHub : MonoBehaviour
{
    private enum GameStates { Running, GameOver, GameSucces}

    private enum GameChunk { Programming, Art, Sound, Publish}

    public enum Difficulty { Easy, Medium, Hard}

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

    //public Difficulty Diffculty => _difficulty;
    //private Difficulty _difficulty;

    [Header("Scene loading")]
    [SerializeField] private string _returnScene;
    [SerializeField] private GameObject _screenSelectPanel;


    private Minigame _playedGame;
    private GameStates _currentState = GameStates.Running;

    void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("Yoni_MinigameTesting"))
        {
            if (!_testmode)
            {
                GameCollection collection = _gameCollections.Aggregate((p, n) => p.Chunk == _currentGameChunck ? p : n);
                _currentMiniGame = collection.GetMinigame();
            }

            InitializeGame();
        }

    }

    public void SetMinigame(Minigame minigame)
    {
        _currentMiniGame = minigame;
    }

    public void SetDifficulty(string difficulty)
    {
        _currentMiniGame.Difficulty = (Difficulty)Enum.Parse(typeof(Difficulty), difficulty);
    }

    public void InitializeGame()
    {
        if (_currentMiniGame != null)
        {
            _currentState = GameStates.Running;
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

    private IEnumerator ReturnToSelectionScreen()
    {
        yield return new WaitForSeconds(.25f);
        var game = FindObjectOfType<Minigame>();
        if (game != null) Destroy(game.gameObject);

        _screenSelectPanel.SetActive(true);
    }

    public void OnGameOver(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                GameInfo.ChangePouchMoneyAmount(1);
                break;
            case Difficulty.Medium:
                GameInfo.ChangePouchMoneyAmount(2);
                break;
            case Difficulty.Hard:
                GameInfo.ChangePouchMoneyAmount(3);
                break;
        }

        _currentState = GameStates.GameOver;
        GameInfo.AddTime(3);
        StartCoroutine(ReturnToSelectionScreen());
    }

    public void OnGameSucces(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                GameInfo.ChangePouchMoneyAmount(2);
                break;
            case Difficulty.Medium:
                GameInfo.ChangePouchMoneyAmount(4);
                break;
            case Difficulty.Hard:
                GameInfo.ChangePouchMoneyAmount(6);
                break;
        }

        _currentState = GameStates.GameSucces;
        GameInfo.AddTime(3);
        StartCoroutine(ReturnToSelectionScreen());

        AudioHub.PlaySound(AudioHub.MinigameSucces);
    }

    public void OnReturn()
    {
        SceneManager.LoadScene(_returnScene);
    }
}