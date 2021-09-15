using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniGame
{
    public class LaserPuzzleEasy : Minigame
    {
        #region Input
        public void InputMouseClick(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                var scaledMousePos = Camera.main.ScreenToWorldPoint(_mousePos);
                var correctMousePos = new Vector3(scaledMousePos.x, scaledMousePos.y, 0);

                RaycastHit2D hit = Physics2D.Raycast(new Vector2(correctMousePos.x, correctMousePos.y), Vector2.zero, 0);
                if (hit)
                {
                    if (hit.collider.CompareTag("Mirror"))
                    {
                        hit.collider.gameObject.transform.Rotate(0, 0, 90);
                    }
                }
            }
        }
        #endregion

        public int Reflections;
        public float MaxLength;

        [SerializeField]
        private LineRenderer _lineRenderer;
        [SerializeField]
        private GameObject _laser;
        private Ray2D _ray;
        private RaycastHit2D _hit;

        private Vector2 _mousePos;
        private PlayerInput _playerInput;
        private InputAction _clickAction;


        private Texture2D _texture;
        public int TextureWidth = 256;
        public int TextureHeight = 256;
        public Renderer PixelPlaneRenderer;
        private Color[] _fillPixels;


        private void Awake()
        {
            _playerInput = FindObjectOfType<PlayerInput>();

            _clickAction = _playerInput.currentActionMap.FindAction("MouseClick");

            _clickAction.performed += InputMouseClick;
        }

        public override void RunGame()
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

            //Vector2 startPoint = new Vector2(0, 0);
            //Vector2 endPoint = new Vector2(20,50);
            //DrawLineAlgorithm((int)startPoint.x, (int)startPoint.y, (int)endPoint.x, (int)endPoint.y);
        }

        public override void UpdateGame()
        {
            LaserCasting(_laser, _lineRenderer);
        }

        private void LaserCasting(GameObject laserObj, LineRenderer laser)
        {
            _ray = new Ray2D(laserObj.transform.position, laserObj.transform.right);

            laser.positionCount = 1;
            laser.SetPosition(0, laserObj.transform.position);
            float remainingLength = MaxLength;

            for (int i = 0; i < Reflections; i++)
            {
                _hit = Physics2D.Raycast(_ray.origin, _ray.direction, remainingLength);
                if (_hit)
                {
                    remainingLength -= Vector2.Distance(_ray.origin, _hit.point);
                    laser.positionCount += 1;
                    laser.SetPosition(laser.positionCount - 1, _hit.point);

                    
                    Vector2 startPoint = new Vector2(laser.GetPosition(0).x, laser.GetPosition(0).y);
                    Vector2 endPoint = new Vector2(laser.GetPosition(1).x, laser.GetPosition(1).y);

                    DrawLineAlgorithm((int)startPoint.x, (int)startPoint.y, (int)endPoint.x, (int)endPoint.y);
                    DrawLineAlgorithm(0, 0, 20, 20);

                    _ray = new Ray2D(_hit.point - _ray.direction * 0.01f, Vector2.Reflect(_ray.direction, _hit.normal));

                    if (_hit.collider.CompareTag("Goal")) Hub.OnGameSucces();

                    if (!_hit.collider.CompareTag("Mirror")) break;
                }
                else
                {
                    laser.positionCount += 1;
                    laser.SetPosition(laser.positionCount - 1, _ray.origin + _ray.direction * remainingLength);
                }
            }
        }

        public override void CheckInput()
        {
            _mousePos = Mouse.current.position.ReadValue();
        }

        // from wikipedia
        private void DrawLineAlgorithm(int x0, int y0, int x1, int y1)
        {
            var dx = Mathf.Abs(x1 - x0);
            var sx = x0 < x1 ? 1 : -1;
            var dy = Mathf.Abs(y1 - y0);
            var sy = y0 < y1 ? 1 : -1;

            var err = dx - dy;

            var loop = true;
            while (loop)
            {
                _texture.SetPixel(x0, y0, Color.red);
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
    }
}

