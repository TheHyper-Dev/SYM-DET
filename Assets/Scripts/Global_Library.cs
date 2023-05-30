using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Global_Library_Data", menuName = "Global_Library")]
public class Global_Library : ScriptableObject
{
    public static Global_Library instance;
    public GameObject messageBox;
    public Item[] items;
}
