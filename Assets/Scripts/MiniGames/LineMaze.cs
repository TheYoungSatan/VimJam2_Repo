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

        [Header("LineSettings")]
        [SerializeField]
        private Transform _startPoint;
        [SerializeField, Range(0.05f, 0.5f)]
        private float _stepValue = 0.1f;
        [SerializeField, Range(0.05f, 0.2f)]
        private float _lineThickness = 0.1f;
        [SerializeField, Range(0.02f, 0.2f)]
        private float _lineSpeed = 0.1f;

        [Header("Other Settings")]
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

        public override void RunGame() 
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

            _inputDir = Directions.Right;

            _timer = 0;
            
            _line.positionCount = 2;
            _line.SetPosition(0, _startPoint.position);
            _line.SetPosition(1, _startPoint.position);
            _line.startWidth = _lineThickness;
            _line.endWidth = _lineThickness;
        }

        public override void UpdateGame()   
        {
            _timer += Time.deltaTime;

            if (_timer >= _lineSpeed)
            {
                LineCreateInterval();
                _timer = 0;
            }
        }

        private void CheckHit(string direction, Vector2 directionvalue, float xOffset, float yOffset)
        {
            if (_inputDir.Equals(direction))
            {
                var hit = Physics2D.Raycast(_line.GetPosition(_line.positionCount - 1), directionvalue, _stepValue, _borderMask);
                if (hit)
                {
                    if (hit.collider.CompareTag("Goal")) Hub.OnGameSucces();
                    else Hub.OnGameOver();
                }
                else
                {
                    var pointOffset = new Vector3(_line.GetPosition(_line.positionCount - 1).x + xOffset,
                                        _line.GetPosition(_line.positionCount - 1).y + yOffset, 0);

                    _line.SetPosition(_line.positionCount - 1, pointOffset);
                }
            }
        }

        private void LineCreateInterval()
        {
            if (_inputDir != null)
            {
                //Checks for hits in any direction
                CheckHit(Directions.Right, transform.right, _stepValue, 0);
                CheckHit(Directions.Left, -transform.right, -_stepValue, 0);
                CheckHit(Directions.Up, transform.up, 0, _stepValue);
                CheckHit(Directions.Down, -transform.up, 0, -_stepValue);
            }
        }
    }
}
