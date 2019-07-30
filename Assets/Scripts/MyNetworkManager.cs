using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{
    [SerializeField] private Button changeSceneButton;
    public static MyNetworkManager Instance { get; private set; }

    private int index;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        if (networkSceneName == "DefaultScene") return;

        print("Spawning player " + index);

        var player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        print(index + " " + player);
        var auxCarController = player.GetComponent<CarController>();
        auxCarController.PlayerIndex = index;

        print(index + " " + auxCarController);
        index++;

        GameManager.Instance.AddPlayer(auxCarController);
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        ClientScene.AddPlayer(conn, 0);
        base.OnServerSceneChanged(networkSceneName);
    }

    public override void OnStartHost()
    {
        index = 0;
        if (changeSceneButton != null)
            changeSceneButton.gameObject.SetActive(true);
        base.OnStartHost();
    }

    public override void OnStopHost()
    {
        if (changeSceneButton != null)
            changeSceneButton.gameObject.SetActive(false);
        base.OnStopHost();
    }

    public void ChooseScene()
    {
        //if (!isServer) return;
        index = 0;
        int rdm = Random.Range(1, 4);
        switch (rdm)
        {
            default:
                ServerChangeScene("Mapa 1");
                NetworkServer.SpawnObjects();

                break;/*
            case 2:
                ServerChangeScene("Mapa 2");
                NetworkServer.SpawnObjects();

                break;
            case 3:
                ServerChangeScene("Mapa 3");
                NetworkServer.SpawnObjects();

                break;*/
        }
        if (changeSceneButton != null)
            changeSceneButton.gameObject.SetActive(false);
    }
}