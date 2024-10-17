using UnityEngine;
using UnityEngine.UI;

namespace SceneManagement
{
    [RequireComponent(typeof(RawImage))]
    public class ExitTransition : MonoBehaviour
    {
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
            _material.SetFloat(_startTimePropertyID, Time.time);
        }
    }
}
