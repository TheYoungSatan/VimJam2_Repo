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

        public GameObject newSprite;

        private int _texSize = 256;

        private void Awake()
        {
            _playerInput = FindObjectOfType<PlayerInput>();

            _clickAction = _playerInput.currentActionMap.FindAction("MouseClick");

            _clickAction.performed += InputMouseClick;
        }

        public override void RunGame()
        {

            //for (int i = 0; i < 20; i++)
            //{
            //    TextureDraw.DrawLine(tex, Random.Range(0, _texSize), Random.Range(0, _texSize), Random.Range(0, _texSize), Random.Range(0, _texSize),
            //        Color.red);
            //}
            
            var tex = new Texture2D(_texSize, _texSize);
            tex.Apply();
            var sprite = Sprite.Create(tex, new Rect(0, 0, _texSize, _texSize), new Vector2(0.5f, 0.5f));
            GameObject testObj = new GameObject("TestObject");
            SpriteRenderer spriteRenderer = testObj.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            //newSprite.GetComponent<Renderer>().material.mainTexture = tex;
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
    }

}
