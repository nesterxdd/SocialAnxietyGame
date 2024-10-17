
using UnityEngine;

public class GameProgression : MonoBehaviour
{
    public static GameProgression instance;
    
    public ChecklistUI checklist;
    public int progressionIndex;
    public NPCController[] mainInteractions;
    public GameObject[] mapIcons;

    private void Awake()
    {
        instance = this;
        
    }

    private void Start()
    {
        progressionIndex = 0;
        foreach (var interaction in mainInteractions)
        {
            interaction.indicatorIcon.SetActive(false);
            interaction.enabled = false;
        }

        foreach (var icon in mapIcons)
        {
            icon.SetActive(false);
        }
        SetNextInteraction();
;
    }

    /// <summary>
    /// Set the next interaction based on the current index
    /// </summary>
    public void SetNextInteraction()
    {
        if (progressionIndex > 6) return;
        mainInteractions[progressionIndex].enabled = true;
        mainInteractions[progressionIndex].indicatorIcon.SetActive(true);
        mainInteractions[progressionIndex].waitingForUser = true;
        mapIcons[progressionIndex].SetActive(true);
        
        checklist.UpdateChecklist(progressionIndex);
    }
    /// <summary>
    /// End the current interaction before starting the next one
    /// </summary>
    public void EndInteraction()
    {
        mainInteractions[progressionIndex].OnConversationEnd.Invoke();
        mainInteractions[progressionIndex].indicatorIcon.SetActive(false);
        mapIcons[progressionIndex].SetActive(false);
        mainInteractions[progressionIndex].waitingForUser = false;
        progressionIndex++;
    }
}
