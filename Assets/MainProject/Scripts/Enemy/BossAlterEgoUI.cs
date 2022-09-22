using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAlterEgoUI : MonoBehaviour
{
    Slider healthBar = null;
    BossAlterEgo dragon = null;

    private void Awake()
    {
        healthBar = GetComponentInChildren<Slider>();

    }

    private void Start()
    {
        dragon = FindObjectOfType<BossAlterEgo>();
    }

    private void Update()
    {
        RefreshHP();
    }

    private void RefreshHP()
    {
        if (dragon != null)
        {
            healthBar.value = dragon.curHealth / dragon.maxHealth;
        }
    }
}