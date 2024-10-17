
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "New Tutorial", menuName = "Tutorial")]
public class TutorialSO : ScriptableObject
{
    [Header("Video")]
    public VideoClip video;
    [Header("Contents")]
    [TextArea(2,10)] public string header;
    [TextArea(4,10)] public string contents;
}
