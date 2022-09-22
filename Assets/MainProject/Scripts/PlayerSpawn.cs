using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject player = null;

    private void Start()
    {
        Instantiate(player, transform.position, transform.rotation);
    }
}
