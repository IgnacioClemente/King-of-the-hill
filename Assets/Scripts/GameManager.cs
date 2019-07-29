using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Color[] colors;
    [SerializeField] Text endGameText;
    [SerializeField] Transform[] spawnPoints;
    
    private List<CarController> players = new List<CarController>();

    private CarController winner;

    public Color[] Colors { get { return colors; } }


    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    public void AddPlayer(CarController player)
    {
        players.Add(player);
        player.transform.position = spawnPoints[player.PlayerIndex].position;
        player.SpawnPoint = spawnPoints[player.PlayerIndex].position;

        player.RpcStartPositions(spawnPoints[player.PlayerIndex].position);
    }

    public void EndGame(CarController winner)
    {
        this.winner = winner;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == winner)
                players[i].RpcEndGame(true);
            else
                players[i].RpcEndGame(false);
        }
    }

    public void SetEndText(bool win)
    {
        if(win)
        {
            endGameText.color = Color.green;
            endGameText.text = "Ganaste";
        }
        else
        {
            endGameText.color = Color.red;
            endGameText.text = "Perdiste";
        }
    }
}
