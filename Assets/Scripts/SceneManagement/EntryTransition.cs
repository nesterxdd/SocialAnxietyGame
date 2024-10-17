using UnityEngine;
using UnityEngine.UI;

namespace SceneManagement
{
    [RequireComponent(typeof(RawImage))]
    public class EntryTransition : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.5f;
        private float _timer;

        private readonly int _startTimePropertyID = Shader.PropertyToID("_StartTime");
        private Material _material;

        private void Awake()
        {
            RawImage image = GetComponent<RawImage>();
            _material = Instantiate(image.material);
            image.material = _material;
        }

        private void OnEnable()
        {
            _timer = 0.0f;
            _material.SetFloat(_startTimePropertyID, Time.time);
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _duration)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
