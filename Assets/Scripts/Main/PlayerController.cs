using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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

    #region Input

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();

        if (_input.x < 0) _model.flipX = true;
        else if (_input.x > 0) _model.flipX = false;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Vector3 checkPos = transform.position;
            checkPos.y += _radius;
            var colls = Physics2D.OverlapCircleAll(checkPos, _radius, _checkLayer);

            if (colls.Length > 0)
            {
                var closest = colls.Aggregate((p, n) => Vector3.Distance(p.transform.position, checkPos) < Vector3.Distance(n.transform.position, checkPos) ? p : n);
                GameObject closestObj = closest.gameObject;
            }
        }
    }

    #endregion

    private void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>();

        _moveAction = _playerInput?.currentActionMap.FindAction("Move");
        _interactAction = _playerInput?.currentActionMap.FindAction("Interact");

        _moveAction.performed += Move;
        _interactAction.performed += Interact;
    }

    private void Update()
    {
        transform.position += (Vector3)_input * Time.deltaTime * _moveSpeed;
    }

    private void OnDrawGizmos()
    {
        Vector3 checkPos = transform.position;
        checkPos.y += _radius;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(checkPos, _radius);
    }
}
