using Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
                if (hit && _targetsLeft != null)
                {
                    if (hit.collider.CompareTag("Target"))
                    {
                        _currentCount++;
                        RemoveCircle(hit.collider.gameObject);
                        SpawnCircle();

                        _targetsLeft.text = (int.Parse(_targetsLeft.text) - 1).ToString(); 
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
        private float _targetSize = 10f;
        [SerializeField]
        private float _amountToWin = 30f;
        [SerializeField]
        private float _maxTime = 20;
        [SerializeField]
        private Transform _parent;
        [SerializeField]
        private Text _targetsLeft;
        [SerializeField]
        private Text _timeLeft;
        [SerializeField]
        private RectTransform _targetCanvas;

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
            if (Difficulty == MinigameHub.Difficulty.Easy) { _targetSize = 1f; _amountToWin = 20; }
            else if (Difficulty == MinigameHub.Difficulty.Medium) { _targetSize /= 2; _amountToWin = 25; }
            else if (Difficulty == MinigameHub.Difficulty.Hard) { _targetSize /= 4; _amountToWin = 30; }

            _targetsLeft.text = _amountToWin.ToString();

            _currentCount = 0;
            _timer = 0;

            StartCoroutine(ConstantSpawnCircle());
        }

        public override void UpdateGame()
        {
            _timer += Time.deltaTime;

            _timeLeft.text = "00:" + Mathf.Round(_maxTime - _timer).ToString("00");

            if (_timer >= _maxTime) Hub.OnGameOver(Difficulty);

            if (_currentCount == _amountToWin) Hub.OnGameSucces(Difficulty);
        }

        private IEnumerator ConstantSpawnCircle()
        {
            for (; ; )
            {
                yield return new WaitForSeconds(_spawnInterval);

                _circles = FindObjectsOfType<CircleCollider2D>();

                if (_circles.Length < 4)
                {
                    Vector3[] screenedges = new Vector3[4];
                    _targetCanvas.GetWorldCorners(screenedges);
                    var randomPos = new Vector3(UnityEngine.Random.Range(screenedges[0].x, screenedges[2].x), UnityEngine.Random.Range(screenedges[0].y, screenedges[2].y), 0);

                    if (Physics2D.Raycast(new Vector2(randomPos.x, randomPos.y), Vector2.zero, 0))
                    {
                        break;
                    }

                    var target = Instantiate(_circlePrefab, randomPos, Quaternion.identity, _parent);
                    target.transform.parent = _targetCanvas;
                    target.transform.localScale = new Vector3(_targetSize, _targetSize, _targetSize);
                }
            }
        }

        private void RemoveCircle(GameObject circleObj)
        {
            circleObj.GetComponent<Animator>().SetTrigger("BugDeath");
            AudioHub.PlaySound(AudioHub.BugDeath);  //gives errors
            Destroy(circleObj, .4f);
        }

        private void SpawnCircle() 
        {
            _circles = FindObjectsOfType<CircleCollider2D>();

            if (_circles.Length < 4)
            {
                Vector3[] screenedges = new Vector3[4];
                _targetCanvas.GetWorldCorners(screenedges);
                var randomPos = new Vector3(UnityEngine.Random.Range(screenedges[0].x, screenedges[2].x), UnityEngine.Random.Range(screenedges[0].y, screenedges[2].y), 0);

                if (Physics2D.Raycast(new Vector2(randomPos.x, randomPos.y), Vector2.zero, 0)) return;

                else 
                {
                    var target = Instantiate(_circlePrefab, randomPos, Quaternion.identity, _parent);
                    target.transform.parent = _targetCanvas;
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
