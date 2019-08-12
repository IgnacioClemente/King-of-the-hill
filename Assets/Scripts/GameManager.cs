using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Color[] colors;
    [SerializeField] float timeToChangeLevel;
    [SerializeField] Text endGameText;
    [SerializeField] Text timer;
    [SerializeField] Image background;
    [SerializeField] Transform[] spawnPoints;
    
    private List<CarController> players = new List<CarController>();

    private CarController winner;
    private bool endGame;

    [SyncVar(hook = "UpdateTimerText")]
    private float timeLeft;

    public Color[] Colors { get { return colors; } }


    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    private void Start()
    {
        timeLeft = timeToChangeLevel;
        timer.text = timeLeft.ToString();
        background.gameObject.SetActive(false);
        background.DOFade(0f, 0f);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Invoke("StartPlayers",0.5f);
    }

    private void Update()
    {
        if (endGame && isServer && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timer.text = ((int)timeLeft).ToString();

            if (timeLeft <= 0)
                MyNetworkManager.Instance.ChooseScene();
        }
    }

    public void StartPlayers()
    {
        players = MyNetworkManager.Instance.Players;
        print(players.Count);
        for (int i = 0; i < players.Count; i++)
        {
            print("inicio de player " + i.ToString() + " en " + spawnPoints[i].position);
            players[i].RpcSetRespawn(spawnPoints[i].position, spawnPoints[i].eulerAngles);
            players[i].RpcRespawn();
        }
    }

    public void EndGame(CarController winner)
    {
        this.winner = winner;
        endGame = true;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == winner)
                players[i].RpcEndGame(true);
            else
                players[i].RpcEndGame(false);
        }
    }

    public void ResetAllPlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].RpcRespawn();
        }
    }

    public void UpdateTimerText(float time)
    {
        this.timeLeft = time;

        timer.text = ((int)timeLeft).ToString();
    }

    public void SetEndText(bool win)
    {
        background.gameObject.SetActive(true);
        background.DOFade(1f, 0.5f);
        timeLeft = timeToChangeLevel;
        ResetAllPlayers();

        if (win)
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
