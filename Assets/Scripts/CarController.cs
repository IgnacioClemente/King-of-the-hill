using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CarController : NetworkBehaviour
{
    [SerializeField] MeshRenderer mesh;
    private NetworkStartPosition[] spawnPoints;
    //public GameObject text_;

    [SyncVar(hook = "SetColor")]
    private int myColorIndex;

    public int MyColorIndex { get { return myColorIndex; } set { myColorIndex = value; } }
    
    void Start()
    {
        spawnPoints = FindObjectsOfType<NetworkStartPosition>();
    }

    public override void OnStartClient()
    {
        SetColor(myColorIndex);
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
                return;
        }

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }

    void FixedUpdate()
    {
        if (transform.rotation == Quaternion.Euler(0, 0, 90) || transform.rotation == Quaternion.Euler(0, 0, -90))
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            Vector3 spawnPoint = Vector3.zero;

            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            transform.position = spawnPoint;
        }
    }

    public void Death()
    {
        if (!isServer)
            return;
        RpcRespawn();
    }

   [Command] public void CmdWhoWon()
    {
        if(isServer)
        {
            //text_.SetActive(true);
        }

        if(isClient)
        {

            //text_.SetActive(false);
        }
    }
    
    public void SetColor(int colorIndex)
    {
        myColorIndex = colorIndex;
        mesh.materials[1].color = GameManager.Instance.Colors[myColorIndex];
    }
}
