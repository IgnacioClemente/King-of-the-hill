using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour 
{
    CarController player;

    private void OnTriggerEnter(Collider other)
    {
        player = other.gameObject.GetComponent<CarController>();
        if (other.gameObject.CompareTag("Player"))
        {
            player.Death();
        }
    }
}
