using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FinalGame
{
    [RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D))]
    public class FinalPlayerController : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private float _jumpForce = 2f;
        [Header("GroundChecking")]
        [SerializeField] private float _checkDistance;
        [SerializeField] private LayerMask _groundLayer;
        [Header("Visuals")]
        [SerializeField] private SpriteRenderer _renderer;

        private PlayerInput _input;
        private Rigidbody2D _rigid;
        private Animator _animator;
        private BoxCollider2D _collider;

        private Vector2 _inputDirections;

        private InputAction _moveAction;
        private InputAction _jumpAction;

        private bool _jump;
        private bool _isGrounded;
        private bool _checkGround;

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
            _rigid = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _collider = GetComponent<BoxCollider2D>();

            if (!_input)
            {
                _input = FindObjectOfType<PlayerInput>();
                _moveAction = _input?.currentActionMap.FindAction("Move");
                _jumpAction = _input?.currentActionMap.FindAction("Jump");

                _moveAction.performed += Move;
                _jumpAction.performed += Jump;
            }
        }

        private void Start()
        {
            _animator.SetInteger("CompletionLevel", (int)GameChanger.instance.PlayerVisualCompletionLevel);
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (!_jump && _isGrounded)
                {
                    _jump = true;
                }
            }
        }

        public void Move(InputAction.CallbackContext context)
        {
            _inputDirections = context.ReadValue<Vector2>();
        }

        private void Update()
        {
            transform.position += new Vector3(_inputDirections.x * Time.deltaTime * _movementSpeed, 0, 0);
            _animator.SetFloat("InputX", Mathf.Abs(_inputDirections.x));

            if (_jump)
            {
                switch (GameChanger.instance.MovementCompletionLevel)
                {
                    case CompletionLevel.Easy:
                        _rigid.gravityScale = 1;
                        _rigid.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                        _jump = false;
                        break;
                    case CompletionLevel.Medium:
                        _jump = false;
                        _rigid.gravityScale = 0;
                        _rigid.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                        break;
                    case CompletionLevel.Hard:
                        _jump = false;
                        _rigid.gravityScale = 1;
                        _rigid.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                        break;
                    case CompletionLevel.Failed:
                        _rigid.gravityScale = 1;
                        _rigid.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                        _jump = false;
                        break;
                }
                _animator.SetBool("IsJumping", true);
                StartCoroutine(WaitForSecond(.25f));
            }

            switch (GameChanger.instance.PlayerVisualCompletionLevel)
            {
                case CompletionLevel.Medium:
                    _renderer.flipX = _inputDirections.x < 0;
                    break;
                case CompletionLevel.Hard:
                    if (_inputDirections.x < 0)
                        _renderer.flipX = true;
                    else if (_inputDirections.x > 0)
                        _renderer.flipX = false;
                    break;
            }

            CheckGround();
            PlayerGravity();

            if (_checkGround)
            {
                if (_isGrounded)
                {
                    _checkGround = false;
                    _animator.SetTrigger("OnLanding");
                    _animator.SetBool("IsJumping", false);
                    _animator.ResetTrigger("OnLanding");
                }
            }
        }

        private void PlayerGravity()
        {
            switch (GameChanger.instance.MovementCompletionLevel)
            {
                case CompletionLevel.Medium:
                    if (!_isGrounded)
                        _rigid.gravityScale += Time.deltaTime * 2f;
                    else
                        _rigid.gravityScale = 1;
                    break;
                case CompletionLevel.Hard:
                    if (_rigid.velocity.y < 0 && !_isGrounded)
                        _rigid.gravityScale = 5f;
                    else
                        _rigid.gravityScale = 1;
                    break;
            }
        }

        private void CheckGround()
        {
            switch (GameChanger.instance.MovementCompletionLevel)
            {
                case CompletionLevel.Easy:
                    _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, _checkDistance, _groundLayer);
                    break;
                case CompletionLevel.Medium:
                    var mediumColls = Physics2D.OverlapCircleAll(transform.position, _checkDistance, _groundLayer);
                    _isGrounded = mediumColls.Length > 0;
                    break;
                case CompletionLevel.Hard:
                    Vector2 checkPos = transform.position;
                    float distanceCheck = _checkDistance;
                    checkPos.y += distanceCheck - .05f;
                    var hardColls = Physics2D.OverlapCircleAll(checkPos, _checkDistance, _groundLayer);
                    _isGrounded = hardColls.Length > 0;
                    break;
                case CompletionLevel.Failed:
                    Vector2 failedcheckPos = transform.position;
                    failedcheckPos.y -= _checkDistance;
                    _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, _checkDistance, _groundLayer);
                    break;
            }
        }

        private IEnumerator WaitForSecond(float time)
        {
            yield return new WaitForSeconds(time);
            _checkGround = true;
        }
    }
}