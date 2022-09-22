using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    Text level = null;
    IStatus player = null;

    private void Awake()
    {
        level = GetComponent<Text>();
    }

    private void Start()
    {
        player = GameManager.Inst.MainPlayer as IStatus;
        RefreshLevel();
        player.OnHealthChange += RefreshLevel;
    }

    private void Update()
    {
        //RefreshLevel();
    }

    private void RefreshLevel()
    {
        if (player != null)
        {
            level.text = $"{player.Level}";
        }
    }
}
