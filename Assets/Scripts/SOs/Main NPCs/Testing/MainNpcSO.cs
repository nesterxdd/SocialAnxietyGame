
using UnityEngine;

[CreateAssetMenu (fileName = "New Main NPC", menuName = "Main NPC")]
public class MainNpcSO : ScriptableObject
{
    [Header("NPC Settings")]
    public string npcName;
    public bool completed;
    [TextArea(4, 10)] public string journalDescription;

    [Header("Introduction")]
    [TextArea(4, 10)] public string opening;
    [TextArea(4, 10)] public string[] playerIntros;
    [TextArea(4, 10)] public string[] introResponses;
    
    [Header("Main Conversation")]
    [TextArea(4, 10)] public string mainDialogue;
    [TextArea(4, 10)] public string choiceDialogue;
    [TextArea(4, 10)] public string[] playerResponses;
    [TextArea(4, 10)] public string[] npcResponses;
    
    [Header("Completion")]
    [TextArea(4, 10)] public string goodbye;
    [TextArea(4, 10)] public string onComplete;

}
