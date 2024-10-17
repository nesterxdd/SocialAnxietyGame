
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotebookUI : MonoBehaviour
{
    private int selectionIndex;
    public int entryIndex;
    public bool newEntry;
    public GameObject newEntryIndicator;
    public NotebookSO[] notebookEntries;
    public NotebookButton[] notebookButtons;
    public TMP_Text entryHeader;
    public TMP_Text entryInfo;
    [SerializeField] private Transform[] textPositions;

    private void Start()
    {
        newEntry = true;
        entryIndex = 1;
        foreach (var button in notebookButtons)
        {
            button.gameObject.SetActive(false);
        }
        
        SetNotebookEntries();
    }

    private void Update()
    {
        if (newEntry)
        {
            newEntryIndicator.SetActive(true);
        }
        else
        {
            newEntryIndicator.SetActive(false);
        }
    }

    /// <summary>
    /// Update the available buttons for the book based on the Entry Num
    /// </summary>
    public void SetNotebookEntries()
    {
        for (int i = 0; i < entryIndex; i++)
        {
            if (i > notebookButtons.Length-1)
            {
                Debug.Log("Out of bounds");
                break;
            }
            notebookButtons[i].gameObject.SetActive(true);
            notebookButtons[i].buttonName.text = notebookEntries[i].buttonName; 
        }
    }
    /// <summary>
    /// Display the contents of the page 
    /// </summary>
    /// <param name="id">Int used to identify which notebook entry to choose from</param>
    public void SetNotebookPage(int id)
    {
        var data = notebookEntries[id];
        entryHeader.text = data.header;
        entryInfo.text = data.info;
        selectionIndex = id;
        TextSlide();
        SetSelectedButton();
    }
    
    /// <summary>
    /// Set the button to selected based on the selection index
    /// </summary>
    private void SetSelectedButton()
    {
        for (int i = 0; i < notebookButtons.Length; i++)
        {
            if (i == selectionIndex)
            {
                notebookButtons[i].selected = true;
            }
            else
            {
                notebookButtons[i].selected = false;
            }
            notebookButtons[i].SetTextColour();
        }
    }
    
    /// <summary>
    /// Animation to slide the text in as a new page is clicked
    /// </summary>
    private void TextSlide()
    {
        entryHeader.transform.position = textPositions[0].position;
        entryInfo.transform.position = textPositions[2].position;
        LeanTween.moveX(entryHeader.gameObject, textPositions[1].position.x, 0.1f);
        LeanTween.moveX(entryInfo.gameObject, textPositions[3].position.x, 0.1f);
    }
}
