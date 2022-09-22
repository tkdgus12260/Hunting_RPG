using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitTest : MonoBehaviour
{
    public float curHealth = 500.0f;
    public float takeExp = 50.0f;
    private Player target;

    private void Awake()
    {
        target = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Debug.Log("피격 ");
            Sword weapon = other.GetComponent<Sword>();

            target.TakeExp(takeExp);
            curHealth -= weapon.damage;
        }
    }
}
