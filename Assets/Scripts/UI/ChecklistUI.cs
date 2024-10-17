
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ChecklistUI : MonoBehaviour
{
    [Header("Current Objective")]
    public TMP_Text currentObjective;
    public Transform[] objectivePositions;
    private Coroutine newObjectiveCoroutine;

    [Header("Checklist")] 
    public MainNpcSO[] npcData;
    public TMP_Text[] journalTexts;
    public TMP_Text[] dashes;
    public Color32[] colours;

    private void Start()
    {
        currentObjective.text = npcData[0].journalDescription;
        currentObjective.GameObject().transform.position = objectivePositions[0].position;
    }

    public void UpdateChecklist(int index)
    {
        for (int i = 0; i < journalTexts.Length; i++)
        {
            if (i < index)
            {
                journalTexts[i].color = colours[1];
                journalTexts[i].text = "<s>" + npcData[i].journalDescription + "<s>";
                dashes[i].color = colours[1];
            }
            else if (i == index)
            {
                journalTexts[i].color = colours[0];
                journalTexts[i].text = npcData[i].journalDescription;
                dashes[i].color = colours[0];
                if (i <= 0) continue;
                if (newObjectiveCoroutine != null)
                {
                    StopCoroutine(newObjectiveCoroutine);
                }
                newObjectiveCoroutine = StartCoroutine(NewObjective(npcData[i].journalDescription));
            }
            else
            {
                journalTexts[i].color = colours[1];
                journalTexts[i].text = npcData[i].journalDescription;
                dashes[i].color = colours[1];
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator NewObjective(string newText)
    {
        currentObjective.text = "<s>" + currentObjective.text + "<s>";
        yield return new WaitForSeconds(2);
        LeanTween.moveY(currentObjective.GameObject(), objectivePositions[1].position.y, 0.1f);
        yield return new WaitForSeconds(1);
        currentObjective.text = newText;
        LeanTween.moveY(currentObjective.GameObject(), objectivePositions[0].position.y, 0.1f);
    }
}
