using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public float delta;
    public float speed;
    private Vector3 startPos;
    private Rigidbody rb;

   private void Start()
   {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
   }

   private void FixedUpdate()
   {
        Vector3 v = startPos;
        v.x += delta * Mathf.Sin(Time.time * speed);
        rb.MovePosition(v);
        //transform.position = v;

    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //collision.transform.SetParent(null);
        }
    }*/
}

