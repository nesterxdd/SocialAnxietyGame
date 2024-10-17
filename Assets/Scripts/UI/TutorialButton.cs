
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [FormerlySerializedAs("buttonName")] public Image button;
    [SerializeField] private Color32[] colors;
    private readonly float transitionTime = 0.2f;
    private Vector3 baseSize = new Vector3(1, 1, 1);

    private void Start()
    {
        transform.localScale = baseSize;
        button.color = colors[0];
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        button.color = colors[1];
        transform.LeanScale(new Vector3(1.03f, 1.03f, 1.03f),transitionTime);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.color = colors[0];
        transform.LeanScale(new Vector3(1, 1, 1),transitionTime);
    }
    private void OnEnable()
    {
        button.color = colors[0];
    }
    private void OnDisable()
    {
        button.color = colors[0];
    }
}
