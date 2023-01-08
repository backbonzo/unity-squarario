using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class TwinStickMovement : MonoBehaviour
{
    [SerializeField] private float _playerSpeed = 5f;
    [SerializeField] private float _controllerDeadzone = 0.1f;
    [SerializeField] private float _gamepadRotateSmoothing = 1000f;

    [SerializeField] private bool _isGamepad;

    private CharacterController _controller;

    private Vector2 _movement;
    private Vector2 _aim;

    private Vector3 _playerVelocity;

    private PlayerControls _playerControls;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _playerControls = new PlayerControls();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        _movement = _playerControls.Controls.Movement.ReadValue<Vector2>();
        _aim = _playerControls.Controls.Aim.ReadValue<Vector2>();
    }

    private void HandleInput()
    {
        var move = new Vector3(_movement.x, _movement.y, 0);
        _controller.Move(move * Time.deltaTime * _playerSpeed);
    }

    private void HandleRotation()
    {
        throw new System.NotImplementedException();
    }
}