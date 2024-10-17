using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Controls
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private float _gamepadSensitivity = 1.0f;
        [SerializeField] private float _mouseSensitivity = 1.0f;
        private Vector3 _offset;
        private float _rotation;
        private bool isDragged;

        private void Awake()
        {
            _offset = transform.position - _parent.position;
        }

        private void LateUpdate()
        {
            transform.position = _parent.position + _offset;
            transform.Rotate(Vector3.up, _rotation * _gamepadSensitivity * 100.0f * Time.deltaTime, Space.World);
        }
        
        public void OnDrag(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case (InputActionPhase.Started):
                    isDragged = true;
                    break;
                case (InputActionPhase.Canceled):
                    isDragged = false;
                    break;
            };
        }

        public void OnLook(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR
            if (context.valueType != typeof(Vector2))
            {
                Debug.LogError($"Invalid type. Should be {typeof(Vector2)} but is {context.valueType}");
                return;
            }
#endif
            if (context.control.device.description.deviceClass.Equals("Mouse"))
            {
                if (!isDragged)
                {
                    return;
                }
                float rotation = context.ReadValue<Vector2>().x;
                transform.Rotate(Vector3.up, rotation * _mouseSensitivity * 0.1f, Space.World);
                return;
            }

            _rotation = context.ReadValue<Vector2>().x;
        }
    }
}
