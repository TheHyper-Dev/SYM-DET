using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MessageBoxData", menuName = "MessageBoxData")]

public class MessageBoxData : ScriptableObject
{
    [TextArea] public string[] messageTexts;
    public int messagePreferenceIndex = 6;
}