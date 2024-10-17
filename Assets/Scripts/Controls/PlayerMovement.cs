using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _walkSpeed = 6f;
        [SerializeField] private float _runSpeed = 12f;
        [SerializeField] private float _jumpPower = 7f;
        [SerializeField] private float _rotationSpeed = 720f;
        [SerializeField] private float _gravity = 9.81f;
        [Space] [SerializeField] private Transform _cameraTransform;

        private bool _isRunning;
        private Vector2 _input;
        private bool _jumping;
        private CharacterController _characterController;
        private float _verticalMovement;

        private bool _canMove = true;

        /// <summary>
        /// Sets player movement on or off.
        /// </summary>
        public bool CanMove
        {
            get => _canMove;
            set => _canMove = value;
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        /// <summary>
        /// Toggles running on and off based on InputAction.State.
        /// </summary>
        /// <param name="context">InputAction context.</param>
        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _isRunning = true;
            }
            else if (context.canceled)
            {
                _isRunning = false;
            }
        }

        /// <summary>
        /// Triggers jump on next FixedUpdate if possible.
        /// </summary>
        /// <param name="context">InputAction context.</param>
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started && _canMove && _characterController.isGrounded)
            {
                _jumping = true;
            }
        }

        /// <summary>
        /// Reads player 2D input from provided context.
        /// </summary>
        /// <param name="context">InputAction context.</param>
        public void OnMove(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR
            if (context.valueType != typeof(Vector2))
            {
                Debug.LogError($"Invalid type. Should be {typeof(Vector2)} but is {context.valueType}");
                return;
            }
#endif
            Vector2 input = context.ReadValue<Vector2>();
            _input = input;
        }

        private void Update()
        {
            Vector3 moveDirection = GetInputDirectionWorldSpace();

            if (moveDirection != Vector3.zero && _canMove)
            {
                transform.forward = Vector3.RotateTowards(
                transform.forward,
                moveDirection.normalized,
                _rotationSpeed * Mathf.Deg2Rad * Time.deltaTime,
                0.0f);
            }
        }

        private void FixedUpdate()
        {
            Vector3 moveDirection;

            if (!_canMove)
            {
                _jumping = false;
                if (_characterController.isGrounded)
                {
                    return;
                }
                _verticalMovement -= _gravity * Time.fixedDeltaTime;
                moveDirection = new Vector3(0.0f, _verticalMovement, 0.0f);
                _characterController.Move(moveDirection * Time.fixedDeltaTime);
                return;
            }

            moveDirection = GetInputDirectionWorldSpace();
            float speed = _isRunning ? _runSpeed : _walkSpeed;
            moveDirection *= speed;

            if (_jumping)
            {
                moveDirection.y = _jumpPower;
                _verticalMovement = _jumpPower;
                _jumping = false;
            }
            else if (!_characterController.isGrounded)
            {
                _verticalMovement -= _gravity * Time.fixedDeltaTime;
                moveDirection.y = _verticalMovement;
            }

            _characterController.Move(moveDirection * Time.fixedDeltaTime);
        }

        private Vector3 GetInputDirectionWorldSpace()
        {
            Vector3 right = _cameraTransform.right;
            Vector3 forward = _cameraTransform.forward;
            Vector3 moveDirection = _input.x * new Vector3(right.x, 0, right.z).normalized;
            moveDirection += _input.y * new Vector3(forward.x, 0, forward.z).normalized;
            return moveDirection;
        }
    }
}
