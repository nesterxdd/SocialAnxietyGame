using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject background;

    private void Start()
    {
        background.SetActive(false);
    }

    private void OnEnable()
    {
        background.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        background.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        background.SetActive(false);
    }
}
