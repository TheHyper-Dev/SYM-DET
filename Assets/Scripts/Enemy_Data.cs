using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Enemy_Data", menuName = "Enemy/Enemy_Data")]
public class Enemy_Data : ScriptableObject
{
    public enum EnemyType : byte { Small, Medium, Large }
    public EnemyType enemyType;

    public float spotting_range = 20f;

}