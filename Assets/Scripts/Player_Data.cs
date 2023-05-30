using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Player_Data", menuName = "Player/Player_Data")]

public class Player_Data : ScriptableObject
{
    public byte LivesLeft = 3;
    [Header("Movement")]
    public float MoveSpeed = 10f;
    public float Acceleration = 5f;
    public float jumpSpeed = 5f;
    public float Gravity = 2f;
    [Header("Animations")]
    public AnimationClip Walk;

    [Header("Audio")]
    public AudioClip[] FootStep_Concrete;
    public AudioClip[] FootStep_Dirt;
    public AudioClip[] FootStep_Metal;

    [Header("Level System")]
    public int XP = 0;
    public AnimationCurve Level_XP_Curve;
}