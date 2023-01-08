using UnityEngine;
using UnityEngine.Animations;
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
        if (_isGamepad)
        {
            // Rotate player
            if (Mathf.Abs(_aim.x) > _controllerDeadzone || Mathf.Abs(_aim.y) > _controllerDeadzone)
            {
                var playerDirection = Vector3.right * _aim.x + Vector3.forward * _aim.y;
                if (playerDirection.sqrMagnitude > 0.0f)
                {
                    var newRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation,
                        _gamepadRotateSmoothing * Time.deltaTime);
                }
            }
        }
        else
        {
            var ray = Camera.main.ScreenPointToRay(_aim);
            var groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(ray, out var rayDistance))
            {
                var point = ray.GetPoint(rayDistance);
                LookAt(point);
            }
        }
    }

    private void LookAt(Vector3 lookPoint)
    {
        var heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }

    public void OnDeviceChange(PlayerInput pi)
    {
        _isGamepad = pi.currentControlScheme.Equals("Gamepad");
    }
}