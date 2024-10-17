using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Notebook", menuName = "Notebook Entry")]
public class NotebookSO : ScriptableObject
{
    public string buttonName;
    public string header;
    [TextArea (4,10)]
    public string info;
}
