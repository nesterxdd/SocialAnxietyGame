using System;
using System.Threading.Tasks;
using Controls;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameManagement
{
    [RequireComponent(typeof(CharacterController))]
    public class PanicAttack : MonoBehaviour
    {
        [SerializeField] private GameObject _startTransition;
        [SerializeField] private float _startTransitionDuration = .5f;
        [SerializeField] private float _blackoutDuration = 1f;
        [SerializeField] private GameObject _endTransition;
        [SerializeField] private float _endTransitionDuration = 0.5f;
        [SerializeField] private GameObject _restartPoint;
        [SerializeField] private PlayerController _playerController;

        private CharacterController _characterController;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public void Invoke()
        {
            _ = HandleAttack();
        }

        private async Task HandleAttack()
        {
            try
            {
                EventSystem eventSystem = EventSystem.current;
                _playerController._closestNpc = null;
                EventSystem.current.enabled = false;
                _startTransition.SetActive(true);
                await Task.Delay((int)((_startTransitionDuration + _blackoutDuration) * 1000.0f));
                _endTransition.SetActive(true);
                _characterController.enabled = false;
                transform.position = _restartPoint.transform.position;
                _characterController.enabled = true;
                _startTransition.SetActive(false);
                await Task.Delay((int)(_endTransitionDuration * 1000.0f));
                eventSystem.enabled = true;
                _endTransition.SetActive(false);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
