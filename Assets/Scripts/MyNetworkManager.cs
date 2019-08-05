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
    private List<CarController> players = new List<CarController>();
    private bool gameStarted = true;

    public List<CarController> Players { get { return players; } }

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

        var player = Instantiate(playerPrefab, new Vector3(100,100,100), playerPrefab.transform.rotation);

        var auxCarController = player.GetComponent<CarController>();
        auxCarController.PlayerIndex = index;
        index++;

        players.Add(auxCarController);

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        print(index + " " + player);

        GameManager.Instance.StartPlayers();
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnServerSceneChanged(networkSceneName);
        print(conn.connectionId);
        print(conn.isConnected);
            ClientScene.AddPlayer(conn, 0);
        //ClientScene.Ready(conn);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {

    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        gameStarted = true;
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