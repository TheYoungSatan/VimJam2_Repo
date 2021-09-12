using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniGame
{
    public static class Directions
    {
        public const string Right = "Right";
        public const string Left = "Left";
        public const string Down = "Down";
        public const string Up = "Up";
    }

    public class LineMaze : Minigame
    {
        #region Input
        public void UpInput(InputAction.CallbackContext context)
        {
            if (context.performed && !_inputDir.Equals(Directions.Down)) //this is so you dont go the opposite direction of what you were going towards
            {
                _line.positionCount += 1;
                _line.SetPosition(_line.positionCount - 1, _line.GetPosition(_line.positionCount - 2));
                _inputDir = Directions.Up;
            }
        }
        public void DownInput(InputAction.CallbackContext context)
        {
            if (context.performed && !_inputDir.Equals(Directions.Up))
            {
                _line.positionCount += 1;
                _line.SetPosition(_line.positionCount - 1, _line.GetPosition(_line.positionCount - 2));
                _inputDir = Directions.Down;
            }
        }
        public void LeftInput(InputAction.CallbackContext context)
        {
            if (context.performed && !_inputDir.Equals(Directions.Right))
            {
                _line.positionCount += 1;
                _line.SetPosition(_line.positionCount - 1, _line.GetPosition(_line.positionCount - 2));
                _inputDir = Directions.Left;
            }
        }
        public void RightInput(InputAction.CallbackContext context)
        {
            if (context.performed && !_inputDir.Equals(Directions.Left))
            {
                _line.positionCount += 1;
                _line.SetPosition(_line.positionCount - 1, _line.GetPosition(_line.positionCount - 2));
                _inputDir = Directions.Right;
            }
        }
        #endregion

        [SerializeField]
        private LineRenderer _line;

        [SerializeField]
        private LayerMask _borderMask;

        private PlayerInput _playerInput;
        private InputAction _upAction;
        private InputAction _downAction;
        private InputAction _leftAction;
        private InputAction _rightAction;

        private float _timer;
        private string _inputDir;

        public override void RunGame() //was public override void RunGame()
        {
            _playerInput = FindObjectOfType<PlayerInput>();

            _upAction = _playerInput.currentActionMap.FindAction(Directions.Up);
            _downAction = _playerInput.currentActionMap.FindAction(Directions.Down);
            _leftAction = _playerInput.currentActionMap.FindAction(Directions.Left);
            _rightAction = _playerInput.currentActionMap.FindAction(Directions.Right);

            _upAction.performed += UpInput;
            _downAction.performed += DownInput;
            _leftAction.performed += LeftInput;
            _rightAction.performed += RightInput;

            _timer = 0;
            _inputDir = Directions.Right;
            var startPoint = new Vector3(-8.89f, 3.06f, 0);

            _line.positionCount = 2;
            _line.SetPosition(0, startPoint);
            _line.SetPosition(1, startPoint);
        }

        public override void UpdateGame()   //was public override void UpdateGame()
        {
            _timer += Time.deltaTime;

            if (_timer >= 0.1f)
            {
                LineCreateInterval();
                _timer = 0;
            }
        }

        private void LineCreateInterval()
        {
            if (_inputDir != null)
            {
                RaycastHit2D hit;

                if (_inputDir.Equals(Directions.Right))
                {
                    if (Physics2D.Raycast(_line.GetPosition(_line.positionCount - 1), transform.right, 0.1f, _borderMask))
                    {
                        Hub.OnGameOver();
                        Debug.Log("Game over");
                    }
                    else
                    {
                        var pointOffset = new Vector3(_line.GetPosition(_line.positionCount - 1).x + 0.1f, _line.GetPosition(_line.positionCount - 1).y, 0);

                        _line.SetPosition(_line.positionCount - 1, pointOffset);
                    }
                }
                else if (_inputDir.Equals(Directions.Left))
                {
                    if (Physics2D.Raycast(_line.GetPosition(_line.positionCount - 1), -transform.right, 0.1f, _borderMask))
                    {
                        Hub.OnGameOver();
                        Debug.Log("Game over");
                    }
                    else
                    {
                        var pointOffset = new Vector3(_line.GetPosition(_line.positionCount - 1).x - 0.1f, _line.GetPosition(_line.positionCount - 1).y, 0);

                        _line.SetPosition(_line.positionCount - 1, pointOffset);

                    }
                }
                else if (_inputDir.Equals(Directions.Up))
                {
                    if (Physics2D.Raycast(_line.GetPosition(_line.positionCount - 1), transform.up, 0.1f, _borderMask))
                    {
                        Hub.OnGameOver();
                        Debug.Log("Game over");
                    }
                    else
                    {
                        var pointOffset = new Vector3(_line.GetPosition(_line.positionCount - 1).x, _line.GetPosition(_line.positionCount - 1).y + 0.1f, 0);

                        _line.SetPosition(_line.positionCount - 1, pointOffset);
                    }
                }
                else if (_inputDir.Equals(Directions.Down))
                {
                    if (Physics2D.Raycast(_line.GetPosition(_line.positionCount - 1), -transform.up, 0.1f, _borderMask))
                    {
                        Hub.OnGameOver();
                        Debug.Log("Game over");
                    }
                    else
                    {
                        var pointOffset = new Vector3(_line.GetPosition(_line.positionCount - 1).x, _line.GetPosition(_line.positionCount - 1).y - 0.1f, 0);
                        _line.SetPosition(_line.positionCount - 1, pointOffset);
                    }
                }
                else return;
            }
        }
    }
}
