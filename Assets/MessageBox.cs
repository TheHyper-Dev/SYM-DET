using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBox : MonoBehaviour
{

    public Animation anim;
    public float Duration = 1f;
    public float TimeStamp = 0f;
    public bool Counting = false;

    void Start()
    {
        anim.Play("MessageFadeIn");
    }

    void FixedUpdate()
    {
        if (anim["MessageFadeIn"].normalizedTime > 0.95f && !Counting)
        {
            Counting = true;
        }
        if (Counting && TimeStamp < Duration)
        {
            TimeStamp += Time.fixedDeltaTime;
        }
        else if (TimeStamp >= Duration && Counting)
        {
            TimeStamp = Duration;
            anim["MessageFadeIn"].speed = -0.5f;
            anim["MessageFadeIn"].normalizedTime = 0.8f;
            anim.Play("MessageFadeIn", PlayMode.StopAll);
            Destroy(gameObject, anim["MessageFadeIn"].length * 1.6f);
            Counting = false;
        }
    }
}
