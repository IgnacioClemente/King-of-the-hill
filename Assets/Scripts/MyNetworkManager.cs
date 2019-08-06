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
        //if (networkSceneName == "DefaultScene") return;

        var player = Instantiate(playerPrefab, new Vector3(100,100,100), playerPrefab.transform.rotation);

        var auxCarController = player.GetComponent<CarController>();
        auxCarController.PlayerIndex = index;
        index++;

        players.Add(auxCarController);

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        print(conn.playerControllers[0].playerControllerId);
        print(NetworkServer.connections.Count);
        print("Spawning player " + index);

        print(index + " " + player);
        GameManager.Instance.StartPlayers();

    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnServerSceneChanged(networkSceneName);
        print(conn.connectionId);
        print(conn.isConnected);
        print(NetworkClient.allClients.Count);
        print(NetworkClient.allClients[0].connection.playerControllers.Count);
        print(conn.playerControllers.Count);
        bool canContinue = true;
        /*
        for (int i = 0; i < NetworkServer.connections.Count; i++)
        {
            for (int j = 0; j < NetworkServer.connections[i].playerControllers.Count; j++)
            {
                print(NetworkServer.connections[i].playerControllers.Count);
                if (NetworkServer.connections[i].playerControllers[i].playerControllerId == conn.connectionId)
                    canContinue = false;
            }
        }*/
        for (int i = 0; i < conn.playerControllers.Count; i++)
        {
            if (conn.playerControllers[i].playerControllerId == conn.connectionId)
                canContinue = false;
        }

        print(canContinue);
        ClientScene.Ready(conn);
        NetworkServer.SpawnObjects();

        if (canContinue)
            ClientScene.AddPlayer(conn, (short)conn.connectionId);

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
                break;
            case 2:
                ServerChangeScene("Mapa 2");
                break;
            case 3:
                ServerChangeScene("Mapa 3");
                break;
        }
        if (changeSceneButton != null)
            changeSceneButton.gameObject.SetActive(false);
    }
}