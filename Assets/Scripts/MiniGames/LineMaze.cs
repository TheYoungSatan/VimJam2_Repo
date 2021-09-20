using Sound;
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
            if (context.performed && !_inputDir.Equals(Directions.Down) && _line != null) //this is so you dont go the opposite direction of what you were going towards
            {
                _line.positionCount += 1;
                _line.SetPosition(_line.positionCount - 1, _line.GetPosition(_line.positionCount - 2));
                _inputDir = Directions.Up;
            }
        }
        public void DownInput(InputAction.CallbackContext context)
        {
            if (context.performed && !_inputDir.Equals(Directions.Up) && _line != null)
            {
                _line.positionCount += 1;
                _line.SetPosition(_line.positionCount - 1, _line.GetPosition(_line.positionCount - 2));
                _inputDir = Directions.Down;
            }
        }
        public void LeftInput(InputAction.CallbackContext context)
        {
            if (context.performed && !_inputDir.Equals(Directions.Right) && _line != null)
            {
                _line.positionCount += 1;
                _line.SetPosition(_line.positionCount - 1, _line.GetPosition(_line.positionCount - 2));
                _inputDir = Directions.Left;
            }
        }
        public void RightInput(InputAction.CallbackContext context)
        {
            if (context.performed && !_inputDir.Equals(Directions.Left) && _line != null)
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
        private float _lineSpeed;

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

        private Texture2D _texture;
        public int TextureWidth = 128;
        public int TextureHeight = 128;
        public Renderer PixelPlaneRenderer;
        private Color[] _fillPixels;

        public MeshCollider PlaneBounds;

        public override void RunGame() 
        {
            if (Difficulty == MinigameHub.Difficulty.Easy) _lineSpeed = 0.15f;
            else if (Difficulty == MinigameHub.Difficulty.Medium) _lineSpeed = 0.1f;
            else if (Difficulty == MinigameHub.Difficulty.Hard) _lineSpeed = 0.05f;

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

            InitializeTexture();
        }

        private void InitializeTexture()
        {
            _texture = new Texture2D(TextureWidth, TextureHeight, TextureFormat.ARGB32, false);
            _texture.filterMode = FilterMode.Point;
            Color fillColor = Color.clear;
            _fillPixels = new Color[TextureWidth * TextureHeight];

            for (int i = 0; i < _fillPixels.Length; i++)
            {
                _fillPixels[i] = fillColor;
            }

            _texture.SetPixels(_fillPixels);
            _texture.Apply();

            PixelPlaneRenderer.material.mainTexture = _texture;
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
                    if (hit.collider.CompareTag("Goal")) Hub.OnGameSucces(Difficulty);
                    else
                    {
                        AudioHub.PlaySound(AudioHub.SnakeDeath);
                        Hub.OnGameOver(Difficulty);
                    }
                }
                else
                {
                    var startpos = new Vector3(_line.GetPosition(_line.positionCount - 1).x, _line.GetPosition(_line.positionCount - 1).y, 0);

                    var pointOffset = new Vector3(startpos.x + xOffset, startpos.y + yOffset, 0);

                    _texture.SetPixels(_fillPixels);
                    _texture.Apply();

                    var widthPlane = PlaneBounds.bounds.size.x;
                    var heightPlane = PlaneBounds.bounds.size.y;

                    var startXCoord = (((widthPlane / 2) + startpos.x) / widthPlane) * TextureHeight;
                    var startYCoord = (((heightPlane / 2) - startpos.y) / heightPlane) * TextureWidth;

                    var endXCoord = (((widthPlane / 2) + pointOffset.x) / widthPlane) * TextureHeight;
                    var endYCoord = (((heightPlane / 2) - pointOffset.y) / heightPlane) * TextureWidth;

                    DrawLineAlgorithm((int)startYCoord, (int)startXCoord, (int)endYCoord, (int)endXCoord, Color.red);

                    _line.SetPosition(_line.positionCount - 1, pointOffset);
                }
            }
        }

        private void DrawLineAlgorithm(int x0, int y0, int x1, int y1, Color color)
        {
            var dx = Mathf.Abs(x1 - x0);
            var sx = x0 < x1 ? 1 : -1;
            var dy = Mathf.Abs(y1 - y0);
            var sy = y0 < y1 ? 1 : -1;

            var err = dx - dy;

            var loop = true;
            while (loop)
            {
                _texture.SetPixel(x0, y0, color);
                if ((x0 == x1) && (y0 == y1)) loop = false;
                var e2 = 2 * err;
                if (e2 > -dy)
                {
                    err = err - dy;
                    x0 = x0 + sx;
                }
                if (e2 < dx)
                {
                    err = err + dx;
                    y0 = y0 + sy;
                }
            }
            _texture.Apply();
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
