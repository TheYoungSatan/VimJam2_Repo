using MiniGame;
using UnityEngine;

public class MinigameHub : MonoBehaviour
{
    private enum GameStates { Running, GameOver, GameSucces}

    [SerializeField] private Minigame _currentMiniGame;
    private Minigame _playedGame;
    private GameStates _currentState = GameStates.Running;

    void Start()
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
