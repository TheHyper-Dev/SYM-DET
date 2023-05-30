using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Item item;
    void Start()
    {

    }

    // Update is called once per frame
    float floatingY = 0f;
    public float floatingSpeed = 6f;
    public float floatingScale = 0.001f;
    void Update()
    {
        Vector3 pos = transform.position;
        floatingY = floatingScale * Mathf.Sin(Time.time * floatingSpeed);
        transform.position = pos + new Vector3(0f, floatingY, 0f);
    }
}
