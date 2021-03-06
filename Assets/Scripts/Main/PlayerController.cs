using Interacting;
using Sound;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour
{
    public static class Directions
    {
        public const string Right = "Right";
        public const string Left = "Left";
        public const string Down = "Down";
        public const string Up = "Up";
    }

    private PlayerInput _playerInput;
    private Animator _animator;
    private InputAction _moveAction;
    private InputAction _interactAction;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;

    [Header("Interact check")]
    [SerializeField] private float _radius = 5f;
    [SerializeField] private LayerMask _checkLayer;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer _model;

    private Vector2 _input;
    private Rigidbody2D _rigid;
    private PlayerInfo _info;
    private bool _inputDelay;

    #region Input

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();

        if (_model != null)
        {
            if (_input.x < 0) _model.flipX = true;
            else if (_input.x > 0) _model.flipX = false;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (_inputDelay) return;

        if (context.performed)
        {
            Vector3 checkPos = transform.position;
            checkPos.y += _radius;
            var colls = Physics2D.OverlapCircleAll(checkPos, _radius, _checkLayer);

            if (colls.Length > 0)
            {
                var closest = colls.Aggregate((p, n) => Vector3.Distance(p.transform.position, checkPos) < Vector3.Distance(n.transform.position, checkPos) ? p : n);
                GameObject closestObj = closest.gameObject;
                IInteractable i = closestObj.GetComponent<IInteractable>();

                if (i is LoadSceneInteraction && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BureauScene"))
                    _info.LivingRoomPlayerPos = _rigid.transform.position;

                i?.OnInteract();
                AudioHub.PlaySound(AudioHub.Interact);
                StartCoroutine("InputDelay");
            }
        }
    }

    #endregion

    public (Transform transform, IInteractable interactable) CheckForInteractables()
    {
        Vector3 checkPos = transform.position;
        var colls = Physics2D.OverlapCircleAll(checkPos, _radius, _checkLayer);
        IInteractable i = null;

        if (colls.Length > 0)
        {
            var closest = colls.Aggregate((p, n) => Vector3.Distance(p.transform.position, checkPos) < Vector3.Distance(n.transform.position, checkPos) ? p : n);
            GameObject closestObj = closest.gameObject;
            i = closestObj.GetComponent<IInteractable>();            
        }

        return (colls.Length > 0 ? (transform, i) : (null, null));
    }

    private void Start()
    {
        GUI.SetActive(true);
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private IEnumerator InputDelay()
    {
        _inputDelay = true;
        yield return new WaitForSeconds(.25f);
        _inputDelay = false;
    }

    private void CheckValues()
    {
        _animator.SetBool("IsSleepy", _info.AwakeTime >= 20);
    }

    private void Update()
    {
        _rigid.position += _input * Time.deltaTime * _moveSpeed;
        _animator.SetFloat("Speed", Mathf.Abs(_input.magnitude));

        if (!_playerInput)
        {
            _playerInput = FindObjectOfType<PlayerInput>();
            _moveAction = _playerInput?.currentActionMap.FindAction("Move");
            _interactAction = _playerInput?.currentActionMap.FindAction("Interact");
            _moveAction.performed += Move;
            _interactAction.performed += Interact;
        }

        if (!_info)
        {
            _info = FindObjectOfType<PlayerInfo>();
        }

        CheckValues();
    }

    private void OnDrawGizmos()
    {
        Vector3 checkPos = transform.position;
        checkPos.y += _radius;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(checkPos, _radius);
    }
}
