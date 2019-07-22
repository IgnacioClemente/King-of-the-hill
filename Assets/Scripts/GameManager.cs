using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Color[] colors;

    public Color[] Colors { get { return colors; } }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;
    }
}
