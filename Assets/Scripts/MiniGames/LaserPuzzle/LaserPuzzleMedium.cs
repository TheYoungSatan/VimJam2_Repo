using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniGame
{
    public class LaserPuzzleMedium : Minigame
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
                        hit.collider.transform.parent.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    }
                }
            }
        }
        #endregion

        public int Reflections;
        public float MaxLength;

        [SerializeField]
        private List<LineRenderer> _lineRenderers;
        [SerializeField]
        private List<GameObject> _lasers;
        [SerializeField]
        private GameObject _movingBlock;
        [SerializeField]
        private Transform _movingBlockStart;
        private Ray2D _ray;
        private RaycastHit2D _hit;

        private Vector2 _mousePos;
        private PlayerInput _playerInput;
        private InputAction _clickAction;

        private float _timer;
        private MovingBlock _blockScript;

        private Texture2D _texture;
        public int TextureWidth = 128;
        public int TextureHeight = 128;
        public Renderer PixelPlaneRenderer;
        private Color[] _fillPixels;

        public MeshCollider PlaneBounds;

        private void Awake()
        {
            _playerInput = FindObjectOfType<PlayerInput>();

            _clickAction = _playerInput.currentActionMap.FindAction("MouseClick");

            _clickAction.performed += InputMouseClick;
        }

        public override void RunGame()
        {
            _timer = 0;
            var block = Instantiate(_movingBlock, _movingBlockStart);
            _blockScript = FindObjectOfType<MovingBlock>();
            _blockScript.CanMove = true;

            InitializeTexture();

            StartCoroutine(RefreshLines()); //dit zorgt ervoor dat er geen lines overblijven waar er geen moeten staan
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
            LaserCasting(_lasers[0], _lineRenderers[0], Color.Lerp(Color.red, Color.yellow, 0.5f));
            LaserCasting(_lasers[1], _lineRenderers[1], Color.green);
        }

        private IEnumerator RefreshLines()
        {
            for (; ; )
            {
                _texture.SetPixels(_fillPixels);
                _texture.Apply();

                yield return new WaitForSeconds(0.3f);
            }
        }

        private void LaserCasting(GameObject laserObj, LineRenderer laser, Color color)
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
                    Vector2 startPoint = new Vector2(laser.GetPosition(laser.positionCount - 1).x, laser.GetPosition(laser.positionCount - 1).y);
                    Vector2 endPoint = new Vector2(_hit.point.x, _hit.point.y);

                    remainingLength -= Vector2.Distance(_ray.origin, _hit.point);
                    laser.positionCount += 1;
                    laser.SetPosition(laser.positionCount - 1, _hit.point);

                    var widthPlane = PlaneBounds.bounds.size.x;
                    var heightPlane = PlaneBounds.bounds.size.y;

                    var startXCoord = (((widthPlane / 2) + startPoint.x) / widthPlane) * TextureHeight;
                    var startYCoord = (((heightPlane / 2) - startPoint.y) / heightPlane) * TextureWidth;

                    var endXCoord = (((widthPlane / 2) + endPoint.x) / widthPlane) * TextureHeight;
                    var endYCoord = (((heightPlane / 2) - endPoint.y) / heightPlane) * TextureWidth;

                    DrawLineAlgorithm((int)startYCoord, (int)startXCoord, (int)endYCoord, (int)endXCoord, color);

                    _ray = new Ray2D(_hit.point - _ray.direction * 0.01f, Vector2.Reflect(_ray.direction, _hit.normal));

                    if (_hit.collider.CompareTag("Goal"))
                    {
                        _timer += Time.deltaTime;
                        if (_timer >= 5)
                        {
                            Hub.OnGameSucces();
                        }
                    }

                    if (_hit.collider.CompareTag("Unlock") && laser == _lineRenderers[1])
                    {
                        _blockScript.CanMove = false;
                    }
                    else if (laser == _lineRenderers[1]) _blockScript.CanMove = true;

                    if (_hit.collider.CompareTag("MovingBlock")) _timer = 0;

                    if (!_hit.collider.CompareTag("Mirror")) break;
                }
                else
                {
                    laser.positionCount += 1;
                    laser.SetPosition(laser.positionCount - 1, _ray.origin + _ray.direction * remainingLength);
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

        public override void CheckInput()
        {
            _mousePos = Mouse.current.position.ReadValue();
        }
    }

}
