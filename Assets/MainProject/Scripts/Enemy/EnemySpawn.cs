using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public enum spawnEnemyType { Slime, Turtle, Dragon};
    public spawnEnemyType enemyType;

    public GameObject slimeEnemy = null;
    public GameObject turtleEnemy = null;
    public GameObject dragonEnemy = null;

    private GameObject instSlime = null;
    private GameObject instTurtle = null;
    private GameObject instDragon = null;

    public float waveInterval  = 3.0f;
    public int   spawnCount    = 3;
    private float respawn = 0.0f;
    private float respawnDelay = 10.0f;

    private void Awake()
    {
        Spawn();
    }

    private void Update()
    {
        ReSpawn();
    }

    //몬스터 사망시 리스폰 함수
    private void ReSpawn()
    {
        switch (enemyType)
        {
            case spawnEnemyType.Slime:
                if (instSlime == null)
                {
                    respawn += Time.deltaTime;
                    if (respawn > respawnDelay)
                    {
                        respawn = 0.0f;
                        SlimeSpawn();
                    }
                }
                break;
            case spawnEnemyType.Turtle:
                if (instTurtle == null)
                {
                    respawn += Time.deltaTime;
                    if (respawn > respawnDelay)
                    {
                        respawn = 0.0f;
                        TurtleSpawn();
                    }
                }
                break;
            case spawnEnemyType.Dragon:
                if (instDragon == null)
                {
                    respawn += Time.deltaTime;
                    if (respawn > respawnDelay)
                    {
                        respawn = 0.0f;
                        DragonSpawn();
                    }
                }
                break;
        }
    }

    // 시작 시 스폰 함수
    private void Spawn()
    {
        switch (enemyType)
        {
            case spawnEnemyType.Slime:
                SlimeSpawn();
                break;
            case spawnEnemyType.Turtle:
                TurtleSpawn();
                break;
            case spawnEnemyType.Dragon:
                DragonSpawn();
                break;
        }
    }

    //슬라임 몬스터 스폰 함수 
    private void SlimeSpawn()
    {
        for (int j = 0; j < waveInterval; j++)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                instSlime = Instantiate(slimeEnemy);
                instSlime.transform.position = this.transform.position;
                instSlime.transform.rotation = this.transform.rotation;
                instSlime.GetComponent<Enemy>().Init(this);
            }
        }
    }
    //거북이 몬스터 스폰 함수 
    private void TurtleSpawn()
    {
        for (int j = 0; j < waveInterval; j++)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                instTurtle = Instantiate(turtleEnemy);
                instTurtle.transform.position = transform.position;
                instTurtle.transform.rotation = transform.rotation;
                instTurtle.GetComponent<Enemy>().Init(this);
            }
        }
    }
    //드래곤 스폰 함수 
    private void DragonSpawn()
    {
        for (int j = 0; j < waveInterval; j++)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                instDragon = Instantiate(dragonEnemy);
                instDragon.transform.position = transform.position;
                instDragon.transform.rotation = transform.rotation;
            }
        }
    }
}
