using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Player mainPlayer = null;

    public Player MainPlayer
    {
        get
        {
            return mainPlayer;
        }
    }

    private CameraMovement mainCamera = null;

    public CameraMovement MainCamera
    {
        get
        {
            return mainCamera;
        }
    }

    private static GameManager instance = null;

    public static GameManager Inst
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            instance.Initialize();
        }
        else
        {
            if(instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void Initialize()
    {
        mainPlayer = FindObjectOfType<Player>();
        mainCamera = FindObjectOfType<CameraMovement>();
    }
}
