using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IconHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.LeanScale(new Vector3(1.05f, 1.05f, 1.05f), 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.LeanScale(new Vector3(1, 1, 1), 0.1f);
    }
}
