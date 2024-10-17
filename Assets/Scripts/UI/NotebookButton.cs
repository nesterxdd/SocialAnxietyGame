using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NotebookButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool unread;
    public bool selected;
    [SerializeField] private NotebookUI nbUI;
    [SerializeField] private int buttonIndex;
    public TMP_Text buttonName;
    [SerializeField] private Color32[] colors;
    private readonly float transitionTime = 0.2f;
    private Vector3 baseSize = new Vector3(1, 1, 1);

    private void Start()
    {
        transform.localScale = baseSize;
        buttonName.color = colors[0];
        unread = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonName.color = colors[1];
        transform.LeanScale(new Vector3(1.05f, 1.05f, 1.05f),transitionTime);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!selected)
        {
            buttonName.color = colors[0];
        }
        transform.LeanScale(new Vector3(1, 1, 1),transitionTime);
    }

    public void OnEnable()
    {
        if (!selected)
        {
            buttonName.color = colors[0];
        }
        transform.LeanScale(new Vector3(1, 1, 1), transitionTime);
    }

    /// <summary>
    /// Change the colour of the text based on if the button is selected
    /// </summary>
    public void SetTextColour()
    {
        buttonName.color = selected ? colors[1] : colors[0];
    }
    
    /// <summary>
    /// Choose a new page based on the button index
    /// </summary>
    public void SelectPage()
    {
        nbUI.SetNotebookPage(buttonIndex);
        unread = false;
        selected = true;    
    }
}
