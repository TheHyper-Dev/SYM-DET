using UnityEngine;
using System;
using TMPro;

public class NPC : MonoBehaviour
{
    public NPC_Data npc_data;
    public int activeMessageIndex = 0;
    bool InteractingWithPlayer = false, collidingWithPlayer = false;
    [SerializeField] TMP_Text messageGO;

    internal int character_index = 0;
    [SerializeField] float time_per_character = 0.2f;
    float timer = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            GameManager.Instance.player.UI.InteractHint.gameObject.SetActive(true);
            collidingWithPlayer = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {

            if (InteractingWithPlayer && character_index < npc_data.messageText[activeMessageIndex].Length)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    timer += time_per_character;
                    character_index++;
                    messageGO.text = npc_data.messageText[activeMessageIndex].Substring(0, character_index); //substring shortcut
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            Close_NPC_Message();
            GameManager.Instance.player.UI.InteractHint.gameObject.SetActive(false);
            collidingWithPlayer = false;
        }
    }
    private void Update()
    {
        if (collidingWithPlayer)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                activeMessageIndex = 0;
                InteractingWithPlayer = true;
                messageGO.transform.parent.gameObject.SetActive(true);
                GameManager.Instance.player.UI.InteractHint.gameObject.SetActive(false);
                GameManager.Instance.player.MovementEnabled = false;
                GameManager.Instance.player.LookEnabled = false;

            }
            //else if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    if (character_index < npc_data.messageText[activeMessageIndex].Length)
            //    {
            //        character_index = npc_data.messageText[activeMessageIndex].Length - 1;
            //    }
            //    else
            //    {
            //        character_index = 0;
            //        activeMessageIndex++;
            //        if (activeMessageIndex >= npc_data.messageText.Length)
            //        {
            //            Close_NPC_Message();
            //            InteractingWithPlayer = false;
            //        }
            //    }
            //}
        }
    }
    void Close_NPC_Message()
    {
        timer = -1f;
        activeMessageIndex = 0;
        messageGO.transform.parent.gameObject.SetActive(false);
        GameManager.Instance.player.MovementEnabled = true;
        GameManager.Instance.player.LookEnabled = true;
    }
}