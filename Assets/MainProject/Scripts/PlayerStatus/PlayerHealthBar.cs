using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    Slider healthBar = null;
    Text healthText = null;
    IStatus player = null;

    private void Awake()
    {
        healthBar = GetComponentInChildren<Slider>();
        healthText = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        player = GameManager.Inst.MainPlayer as IStatus;
        RefreshHP();
        player.OnHealthChange += RefreshHP;
    }

    private void RefreshHP()
    {
        if (player != null)
        {
            healthBar.value = player.HP / player.MaxHP;
            //healthText.text = $"{player.HP}";
            healthText.text = $"{player.HP.ToString("N0")}";
        }
    }

}
