using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Utility
{
    public class Delay : MonoBehaviour
    {
        [SerializeField] private float _delay;
        [SerializeField] private UnityEvent _onElapsed;
        private float _timer;

        private void OnEnable()
        {
            _timer = 0;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _delay)
            {
                _onElapsed.Invoke();
                enabled = false;
            }
        }
    }
}
