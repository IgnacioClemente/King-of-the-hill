using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CheckWin : NetworkBehaviour
{
    CarController player;

    void OnTriggerEnter(Collider other)
    {
        player = other.gameObject.GetComponent<CarController>();
        if (other.gameObject.CompareTag("Player"))
        {
            if(isClient)
                player.CmdWhoWon();
        }
    }
}
