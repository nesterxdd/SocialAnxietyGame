using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyButtonsManager : MonoBehaviour
{
    [SerializeField] private NotebookButton[] buttons;
    private int selectionIndex = 1;

    private void Start()
    {
        SetSelectedButton();
    }
    
    /// <summary>
    /// Set the index for the current difficulty
    /// </summary>
    /// <param name="index">Index of the button in the array</param>
    public void SetSelectionIndex(int index)
    {
        selectionIndex = index;
    }
    
    /// <summary>
    /// Set the current selected difficulty button and highlight it
    /// </summary>
    public void SetSelectedButton()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == selectionIndex)
            {
                buttons[i].selected = true;
                buttons[i].SetTextColour();
            }
            else
            {
                buttons[i].selected = false;
                buttons[i].SetTextColour();
            }
        }
    }


}
