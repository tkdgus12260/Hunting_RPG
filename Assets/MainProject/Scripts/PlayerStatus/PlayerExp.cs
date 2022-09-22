using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExp : MonoBehaviour
{
    Slider playerExp = null;
    IStatus player = null;

    private void Awake()
    {
        playerExp = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        player = GameManager.Inst.MainPlayer as IStatus;
        RefreshExp();
        player.OnExpChange += RefreshExp;
    }

    private void RefreshExp()
    {
        if (player != null)
        {
            playerExp.value = player.EXP / player.MaxEXP;
        }
    }
}
