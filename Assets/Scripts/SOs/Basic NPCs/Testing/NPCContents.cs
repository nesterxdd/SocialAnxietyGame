
using System;
using UnityEngine;
[CreateAssetMenu(fileName = "New Basic NPC", menuName = "Basic NPC")]
public class NPCContents : ScriptableObject
{
    [Header("NPC Information")]
    public string npcName;
    public BasicConversationType conversationType; 
    public bool completed;

    [Header("Single Dialogue")]
    [TextArea(3, 10)]
    public string[] dialogue;

    [Header("Choice Dialogue")] 
    [TextArea(3, 10)] public string opening;
    [TextArea(3, 10)] public string[] choices;
    [TextArea(3, 10)] public string[] responses;
    [TextArea(3, 10)] public string ending;
    [TextArea(3, 10)] public string onComplete;
    
    public enum BasicConversationType
    {
        Single,
        Choice
    }
}
