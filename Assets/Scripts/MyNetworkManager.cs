using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{
    [SerializeField] NetworkStartPosition[] spawnPoints;

    private int index;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var player = Instantiate(playerPrefab, spawnPoints[index].transform.position, playerPrefab.transform.rotation);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        var auxCarController = player.GetComponent<CarController>();
        auxCarController.PlayerIndex = index;
        auxCarController.SpawnPoint = spawnPoints[index].transform.position;

        index++;

        GameManager.Instance.AddPlayer(auxCarController);
    }
}