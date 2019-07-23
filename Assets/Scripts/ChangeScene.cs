using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
public class ChangeScene : NetworkBehaviour
{
    private void Awake()
    {
        int rdm = Random.Range(1,4);
        NetworkManager net;
        switch (rdm)
        {
            case 1:
                net = GetComponent<NetworkManager>();
                net.ServerChangeScene("Mapa 1");
                NetworkServer.SpawnObjects();

                break;
            case 2:
                net = GetComponent<NetworkManager>();
                net.ServerChangeScene("Mapa 2");
                NetworkServer.SpawnObjects();

                break;
            case 3:
                net = GetComponent<NetworkManager>();
                net.ServerChangeScene("Mapa 3");
                NetworkServer.SpawnObjects();

                break;

        }
    }
}
