using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniGame
{
    public class LaserPuzzleHard : Minigame
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
        private List<LineRenderer> _lineRenderers;
        [SerializeField]
        private List<GameObject> _lasers;

        [SerializeField]
        private GameObject _movingBlock;
        [SerializeField]
        private List<Transform> _movingBlockStarts;
        private Ray2D _ray;
        private RaycastHit2D _hit;

        private Vector2 _mousePos;
        private PlayerInput _playerInput;
        private InputAction _clickAction;

        private float _timer;
        private List<MovingBlock> _blockScripts = new List<MovingBlock>();

        private void Awake()
        {
            _playerInput = FindObjectOfType<PlayerInput>();

            _clickAction = _playerInput.currentActionMap.FindAction("MouseClick");

            _clickAction.performed += InputMouseClick;
        }

        public override void RunGame()
        {
            _timer = 0;

            for (int i = 0; i < 3; i++)
            {
                var blockObj = Instantiate(_movingBlock, _movingBlockStarts[i]);
                _blockScripts.Add(blockObj.GetComponent<MovingBlock>());
                _blockScripts[i].CanMove = true;
            }
        }

        public override void UpdateGame()
        {
            LaserCasting(_lasers[0], _lineRenderers[0]);
            LaserCastingWithBlock(_lasers[1], _lineRenderers[1], _blockScripts[0]);
            LaserCastingWithBlock(_lasers[2], _lineRenderers[2], _blockScripts[1]);
            LaserCastingWithBlock(_lasers[3], _lineRenderers[3], _blockScripts[2]);
        }

        private void LaserCastingWithBlock(GameObject laserObj, LineRenderer laser, MovingBlock movingBlock)
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
                        _blockScripts[0].CanMove = false;
                    }
                    else if (_hit.collider.CompareTag("Unlock") && laser == _lineRenderers[2])
                    {
                        _blockScripts[1].CanMove = false;
                    }
                    else if (_hit.collider.CompareTag("Unlock") && laser == _lineRenderers[3])
                    {
                        _blockScripts[2].CanMove = false;
                    }
                    else if (laser == _lineRenderers[1] || laser == _lineRenderers[2] || laser == _lineRenderers[3]) movingBlock.CanMove = true;

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

                    if (_hit.collider.CompareTag("Goal"))
                    {
                        _timer += Time.deltaTime;
                        if (_timer >= 5)
                        {
                            Hub.OnGameSucces();
                        }
                    }

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

        public override void CheckInput()
        {
            _mousePos = Mouse.current.position.ReadValue();
        }
    }

}
