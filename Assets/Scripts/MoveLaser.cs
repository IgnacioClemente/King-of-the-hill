using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLaser : MonoBehaviour
{
    public float delta;
    public float speed;
    private Vector3 startPos;

   private void Start()
    {
        startPos = transform.position;
    }
   private void Update()
    {
        Vector3 v = startPos;
        v.x += delta * Mathf.Sin(Time.time * speed);
        v.z -= delta * Mathf.Sin(Time.time * speed);
        transform.position = v;
    }
}