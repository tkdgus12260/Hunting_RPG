using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { Slime, Turtle}
    public Type enemyType;
    public float curHealth = 200.0f;
    public float maxHealth = 200.0f;
    public float takeExp = 10.0f;

    public BoxCollider meleeArea;
    private Rigidbody rigid;
    private NavMeshAgent nav;
    private Animator ani;
    private EnemySpawn spawn;
    public GameObject dropItem;

    private bool isChase;
    private bool isAttack;
    public bool isOut;
    private bool isDie;

    private int droppercentage = 3;
    private float attackDis = 5.0f;
    private float returnDis = 20.0f;
    private float attackDelay = 2.0f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        spawn = FindObjectOfType<EnemySpawn>();
    }

    private void Update()
    {
        if (!GameManager.Inst.MainPlayer.isDeath)
        {
            FollowTarget();

            if (nav.enabled)
            {
                nav.SetDestination(GameManager.Inst.MainPlayer.transform.position);
                ani.SetBool("isWalk", true);
            }
        }

        TargetDeath();
    }

    private void FixedUpdate()
    {
        if(!GameManager.Inst.MainPlayer.isDeath)
            Targetting();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Sword weapon = other.GetComponent<Sword>();
            curHealth -= weapon.damage;
            StartCoroutine(OnDamage());
            GameManager.Inst.MainPlayer.enemyHitClip.Play();
        }
    }

    // 몬스터 피격 코루틴
    IEnumerator OnDamage()
    {
        nav.enabled = false;
        ani.SetBool("isHit", true);
        yield return new WaitForSeconds(0.22f);
        ani.SetBool("isHit", false);
        nav.enabled = true;

        // 몬스터 사망 시
        if (curHealth <= 0)
        {
            OnDie();
        }
    }

    // 몬스터 사망 시 함수
    private void OnDie()
    {

        ani.SetTrigger("onDie");
        gameObject.layer = 7;
        GameManager.Inst.MainPlayer.TakeExp(takeExp);

        switch (enemyType)
        {
            case Type.Slime:
                nav.enabled = false;
                isDie = true;
                meleeArea.enabled = false;
                break;
            case Type.Turtle:
                nav.enabled = false;
                isDie = true;
                meleeArea.enabled = false;
                break;
        }

        DropItem();
        Destroy(gameObject, 2.5f);
    }

    // 아이템 드랍
    private void DropItem()
    {
        int percentage = Random.Range(0, 10);

        if(percentage <= droppercentage)
        {
            Instantiate(dropItem, transform.position, transform.rotation);
        }
    }

    // 몬스터 플레이어 인식 함수
    private void FollowTarget()
    {
        float targetDistance = Vector3.Distance(transform.position, GameManager.Inst.MainPlayer.transform.position);
        float spawnDistance = Vector3.Distance(transform.position, spawn.transform.position);

        if (targetDistance < attackDis && !isOut && !isDie)
        {
            nav.enabled = true;
        }

        if (spawnDistance > returnDis)
        {
            isOut = true;
            nav.enabled = false;
        }
        else if (spawnDistance < 0.5f)
        {
            isOut = false;
            ani.SetBool("isWalk", false);
        }

        if (isOut == true)
        {
            transform.LookAt(spawn.gameObject.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, spawn.transform.position, 5 * Time.deltaTime);
        }
    }

    // 되돌아갈 스폰 포인트 저장
    public void Init(EnemySpawn s) { spawn = s; }

    // 몬스터 플레이어 공격 인식 함수
    private void Targetting()
    {
        float targetRadius = 0;
        float targetRange = 0;

        switch (enemyType)
        {
            case Type.Slime:
                targetRadius = 1.0f;
                targetRange = 1.0f;
                break;
            case Type.Turtle:
                targetRadius = 1.0f;
                targetRange = 1.0f;
                break;
        }
        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, targetRadius,
                                    transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack)
        {
            StartCoroutine("Attack");
        }
    }

    // 몬스터 공격 코루틴
    IEnumerator Attack()
    {
        isAttack = true;
        transform.LookAt(GameManager.Inst.MainPlayer.transform.position);

        switch (enemyType)
        {
            case Type.Slime:
                yield return new WaitForSeconds(0.5f);
                ani.SetTrigger("onAttack");
                yield return new WaitForSeconds(attackDelay);
                break;
            case Type.Turtle:
                yield return new WaitForSeconds(0.5f);
                ani.SetTrigger("onAttack");
                yield return new WaitForSeconds(attackDelay);
                break;
        }

        isAttack = false;
    }

    public void AttackOn()
    {
        meleeArea.enabled = true;
    }
    public void AttackOff()
    {
        meleeArea.enabled = false;
    }

    private void TargetDeath()
    {
        if (GameManager.Inst.MainPlayer.isDeath)
        {
            nav.enabled = false;
            ani.SetBool("isWalk", false);
        }
    }
}
