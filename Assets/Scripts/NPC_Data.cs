using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC_Data", menuName = "NPC_Data")]
public class NPC_Data : ScriptableObject
{
    public GameObject messageBox;
    [TextArea] public string[] messageText;
    public int messagePreferenceIndex = 6;
    [TextArea] public string[] playerAnswersText;
    public int playerAnswersPreferenceIndex = 0;
}
