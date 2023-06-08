using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{

    public Animation anim;
    public MessageBoxData data;
    public int activeMessageIndex = 0;
    internal int character_index = 0;
    [SerializeField] float time_per_character = 0.2f;
    float timer = 0f;
    public bool revealing_message = false;
    [SerializeField] TMP_Text message_TEXT;
    IEnumerator Start()
    {

        yield return new WaitForSeconds(2.5f);
        anim.Play("MessageFadeIn");
        revealing_message = true;
        message_TEXT.enabled = true;
    }
    private void Update()
    {
        if (revealing_message && character_index < data.messageTexts[activeMessageIndex].Length)
        {

            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer += time_per_character;
                character_index++;
                message_TEXT.text = data.messageTexts[activeMessageIndex].Substring(0, character_index); //substring shortcut, nvm
            }
        }
        else if (revealing_message && character_index >= data.messageTexts[activeMessageIndex].Length)
        {
            revealing_message = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (character_index < data.messageTexts[activeMessageIndex].Length)
            {
                character_index = data.messageTexts[activeMessageIndex].Length - 1;
            }
            else //finished revealing
            {
                character_index = 0;
                if (activeMessageIndex < data.messageTexts.Length - 1)
                {
                    activeMessageIndex++;
                    timer = -1f;
                    revealing_message = true;
                }
                else
                {
                    revealing_message = false;
                    GameManager.Instance.scene_anim.Play("scene_fade_out");
                    anim.Play("MessageFadeOut");
                    //activeMessageIndex = 0;
                    StartCoroutine(GameManager.load_level(GameManager.Instance.scene_anim["scene_fade_out"].length, GameManager.Instance.level_index + 1));
                }
            }
        }
    }
}
