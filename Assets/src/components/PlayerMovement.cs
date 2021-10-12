using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _walkingSpeed = 7.5f;
    
    [SerializeField]
    private float _runningSpeed = 11.5f;
    
    [SerializeField]
    private float _jumpSpeed = 8.0f;
    
    [SerializeField]
    private float _gravity = 20.0f;
    
    [SerializeField]
    private float _lookSpeed = 2.0f;
    
    [SerializeField]
    private float _lookXLimit = 60.0f;
    
    [SerializeField]
    private Camera _playerCamera;
    
    [SerializeField]
    private CharacterController _characterController;

    private Vector3 _moveDirection = Vector3.zero;
    private float _rotationX = 0;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Vector3 forwardDirection = transform.TransformDirection(Vector3.forward);
        Vector3 rightDirection = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = (isRunning ? _runningSpeed : _walkingSpeed) * Input.GetAxis("Vertical");
        float curSpeedY = (isRunning ? _runningSpeed : _walkingSpeed) * Input.GetAxis("Horizontal");
        float movementDirectionY = _moveDirection.y;
        _moveDirection = (forwardDirection * curSpeedX) + (rightDirection * curSpeedY);

        if (Input.GetButton("Jump") && _characterController.isGrounded)
        {
            _moveDirection.y = _jumpSpeed;
        }
        else
        {
            _moveDirection.y = movementDirectionY;
        }

        if (!_characterController.isGrounded)
        {
            _moveDirection.y -= _gravity * Time.deltaTime;
        }

        _characterController.Move(_moveDirection * Time.deltaTime);

        _rotationX += -Input.GetAxis("Mouse Y") * _lookSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -_lookXLimit, _lookXLimit);

        _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _lookSpeed, 0);
    }
}