using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{
    [SerializeField] private Button changeSceneButton;

    private int index;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        if (networkSceneName == "DefaultScene") return;

        NetworkServer.SpawnObjects();
        print("Spawning player " + index);

        var player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        var auxCarController = player.GetComponent<CarController>();
        auxCarController.PlayerIndex = index;

        index++;

        GameManager.Instance.AddPlayer(auxCarController);
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        ClientScene.AddPlayer(conn, 0);
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        if(changeSceneButton)
            changeSceneButton.gameObject.SetActive(true);
    }

    public void ChooseScene()
    {
        //if (!isServer) return;

        int rdm = Random.Range(1, 4);
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