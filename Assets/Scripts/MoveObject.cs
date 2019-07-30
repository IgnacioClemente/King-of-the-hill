using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MoveObject :  NetworkBehaviour
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
        if (isServer)
        {
            Vector3 v = startPos;
            v.x += delta * Mathf.Sin(Time.time * speed);
            rb.MovePosition(v);
            RpcMove(v);
        }
        //transform.position = v;
    }

    [ClientRpc]
    public void RpcMove(Vector3 newPos)
    {
        if(rb) rb.MovePosition(newPos);
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

