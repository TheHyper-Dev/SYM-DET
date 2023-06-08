using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] Global_Library global_Lib;
    public Player player;
    public Animation scene_anim;
    public GameObject messageBox;
    public int level_index = 1;
    private void Start()
    {
        Instance = this;
        Global_Library.instance = global_Lib;
        scene_anim.Play("scene_fade_in");
    }
    public static IEnumerator load_level(float delay, int level_index)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Level_" + level_index);
    }
}
