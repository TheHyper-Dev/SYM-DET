using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] Global_Library global_Lib;
    public Player player;
    public Animation scene_anim;

    private void Awake()
    {
        Instance = this;
        Global_Library.instance = global_Lib;
        scene_anim.Play("scene_fade_in");
    }
}
