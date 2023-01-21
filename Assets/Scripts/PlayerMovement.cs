using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public GameObject foodPrefab;
    [SerializeField] private float _speed = 40.0f;
    [SerializeField] private Camera _camera;
    private PlayerInputs _input;
    private Vector2 _movement;
    private Vector2 _mousePos;

    private Rigidbody2D _rigidBody;

    private Vector3 _testPos;

    private void Awake()
    {
        _input = new PlayerInputs();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _input.Enable();

        _input.Gameplay.Movement.performed += OnMovement;
        _input.Gameplay.Movement.canceled += OnMovement;

        _input.Gameplay.MousePos.performed += OnMousePos;
        _input.Gameplay.MousePos.canceled += OnMousePos;

        _input.Gameplay.Click.performed += OnClick;
        _input.Gameplay.Click.canceled += OnClick;
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
    }

    private void OnMousePos(InputAction.CallbackContext context)
    {
        _mousePos = context.ReadValue<Vector2>();
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        //var mousePosClicked = context.ReadValue<Vector2>();
        Instantiate(foodPrefab, _testPos, Quaternion.identity);
    }

    private void FixedUpdate()
    {
        _rigidBody.AddForce(_movement * _speed);

        var pos = _camera.ScreenToWorldPoint(_mousePos);
        var posVec2 = new Vector2(pos.x, pos.y);
        _testPos = new Vector3(posVec2.x, posVec2.y, -1);

        var facingDirection = posVec2 - _rigidBody.position;
        var angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        _rigidBody.MoveRotation(angle);
    }
}