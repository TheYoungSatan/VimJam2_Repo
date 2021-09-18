using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Sound;

namespace MiniGame
{
    public class NumberMemory : Minigame
    {
        [SerializeField] private Text _taskText;
        [SerializeField] private Text _inputText;

        [SerializeField] private int _numberAmount;
        [SerializeField] private float _displayTime;

        private string _task = "";
        private string _input = "";

        private bool _runGame = false;

        public override void RunGame()
        {
            if (Difficulty == MinigameHub.Difficulty.Easy) { _numberAmount = 4; _displayTime = 2f; }
            else if (Difficulty == MinigameHub.Difficulty.Medium) { _numberAmount = 6; _displayTime = 2f; } 
            else if (Difficulty == MinigameHub.Difficulty.Hard) { _numberAmount = 8; _displayTime = 2.5f; }
            
            for (int i = 0; i < _numberAmount; i++)
                _task += UnityEngine.Random.Range(0, 10);

            _taskText.text = _task;
            Keyboard.current.onTextInput += ReadInput;

            StartCoroutine(DisplayTime());
        }

        public void ReturnButton() => Hub.OnReturn();

        private void ReadInput(char obj)
        {
            if (!_runGame) return;
            if (int.TryParse(obj.ToString(), out int n) && _input.Length < _numberAmount)
            {
                _input += n.ToString();
                AudioHub.PlaySound(AudioHub.InputSound);

                _inputText.text = _input;

                if (_input != _task.Substring(0, _input.Length))
                {
                    Hub.OnGameOver();
                    _runGame = false;
                    _taskText.text = _task;
                }
                else if (_input.Length == _task.Length && _input == _task)
                {
                    Hub.OnGameSucces();
                    _taskText.text = _task;
                }
            }
        }

        IEnumerator DisplayTime()
        {
            yield return new WaitForSeconds(_displayTime);
            string s = "";
            for (int i = 0; i < _numberAmount; i++)
                s += "_ ";

            _taskText.text = s;
            _runGame = true;
        }
    }
}
