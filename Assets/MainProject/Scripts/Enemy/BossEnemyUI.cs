using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemyUI : MonoBehaviour
{
    Slider healthBar = null;
    BossEnemy dragon = null;

    private void Awake()
    {
        healthBar = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        dragon = FindObjectOfType<BossEnemy>();
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
