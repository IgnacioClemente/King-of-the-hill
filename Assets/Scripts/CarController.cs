using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CarController : NetworkBehaviour
{
    [SerializeField] MeshRenderer mesh;

    [SyncVar(hook = "SetColor")]
    private int playerIndex;

    [SyncVar(hook = "SetPosition")]
    public Vector3 SpawnPosition;
    [SyncVar]
    public Vector3 SpawnRotation;

    public int PlayerIndex { get { return playerIndex; } set { playerIndex = value; } }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public override void OnStartClient()
    { 
        SetColor(playerIndex);
        SetPosition(SpawnPosition);
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
    }
    
    [ClientRpc]
    public void RpcEndGame(bool winner)
    {
        if (!isLocalPlayer)
            return;

        GameManager.Instance.SetEndText(winner);
    }


    [ClientRpc]
    public void RpcSetRespawn(Vector3 position, Vector3 rotation)
    {
        SpawnPosition = position;
        SpawnRotation = rotation;
    }

    [ClientRpc]
    public void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            transform.position = SpawnPosition;
            transform.eulerAngles = SpawnRotation;
        }
    }

    public void Death()
    {
        if (!isServer)
            return;
        RpcRespawn();
    }

    [Command]
    public void CmdWhoWon()
    {
        if(isServer)
        {
            GameManager.Instance.EndGame(this);
        }
    }
    
    public void SetColor(int colorIndex)
    {
        playerIndex = colorIndex;
        mesh.materials[1].color = GameManager.Instance.Colors[playerIndex];
    }

    public void SetPosition(Vector3 position)
    {
        SpawnPosition = position;
    }
}
