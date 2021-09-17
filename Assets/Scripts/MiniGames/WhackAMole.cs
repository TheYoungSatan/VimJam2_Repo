using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace MiniGame
{
    public class WhackAMole : Minigame
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
                    if (hit.collider.CompareTag("Target"))
                    {
                        _currentCount++;
                        RemoveCircle(hit.collider.gameObject);
                        SpawnCircle();
                    }
                }
            }
        }
        #endregion

        private Vector2 _mousePos;
        public GameObject _circlePrefab;

        [Header("GameSettings")]
        [SerializeField]
        private float _spawnInterval = 1;
        [SerializeField]
        private float _targetSize = 0.5f;
        [SerializeField]
        private float _amountToWin = 30f;
        [SerializeField]
        private float _maxTime = 30;

        private float _timer;
        private float _currentCount;

        private PlayerInput _playerInput;
        private InputAction _clickAction;

        private CircleCollider2D[] _circles;
        public Transform Centre;

        private Vector2 _screenEdgesX;
        private Vector2 _screenEdgesY;

        private void Awake()
        {
            _playerInput = FindObjectOfType<PlayerInput>();

            _clickAction = _playerInput.currentActionMap.FindAction("MouseClick");

            _clickAction.performed += InputMouseClick;
        }

        public override void RunGame()
        {
            var pos = Camera.main.ScreenToWorldPoint(Centre.position);
            var offset = new Vector2(4.3f - _targetSize, 2.25f - _targetSize);

            _screenEdgesX = new Vector2(pos.x - offset.x, pos.x + offset.x);
            _screenEdgesY = new Vector2(pos.y + offset.y, pos.y - offset.y);

            _currentCount = 0;
            _timer = 0;
            StartCoroutine(ConstantSpawnCircle());
        }

        public override void UpdateGame()
        {
            _timer += Time.deltaTime;

            if (_timer >= _maxTime) Hub.OnGameOver();

            if (_currentCount == _amountToWin) Hub.OnGameSucces();
        }

        private IEnumerator ConstantSpawnCircle()
        {
            for (; ; )
            {
                yield return new WaitForSeconds(_spawnInterval);

                _circles = FindObjectsOfType<CircleCollider2D>();

                if (_circles.Length < 4)
                {
                    var randomPos = new Vector3(Random.Range(_screenEdgesX.x, _screenEdgesX.y), Random.Range(_screenEdgesY.x, _screenEdgesY.y), 0);

                    if (Physics2D.Raycast(new Vector2(randomPos.x, randomPos.y), Vector2.zero, 0))
                    {
                        break;
                    }

                    var target = Instantiate(_circlePrefab, randomPos, Quaternion.identity);
                    target.transform.localScale = new Vector3(_targetSize, _targetSize, _targetSize);
                }
            }
        }

        private void RemoveCircle(GameObject circleObj)
        {
            Destroy(circleObj);
        }

        private void SpawnCircle() 
        {
            _circles = FindObjectsOfType<CircleCollider2D>();

            if (_circles.Length < 4)
            {
                var randomPos = new Vector3(Random.Range(_screenEdgesX.x, _screenEdgesX.y), Random.Range(_screenEdgesY.x, _screenEdgesY.y), 0);

                if (Physics2D.Raycast(new Vector2(randomPos.x, randomPos.y), Vector2.zero, 0)) return;

                else 
                {
                    var target = Instantiate(_circlePrefab, randomPos, Quaternion.identity);
                    target.transform.localScale = new Vector3(_targetSize, _targetSize, _targetSize);
                }
            }
        }

        public override void CheckInput()
        {
            _mousePos = Mouse.current.position.ReadValue();
        }
    }

}
