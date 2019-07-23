using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Color[] colors;
    [SerializeField] Text endGameText;

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
