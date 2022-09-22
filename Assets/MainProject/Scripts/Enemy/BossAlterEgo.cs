using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAlterEgo : MonoBehaviour
{

    public float curHealth = 500.0f;
    public float maxHealth = 500.0f;

    public float rushAttack = 0.0f;
    public float rushAttackDelay = 15.0f;
    private float backSpped = 3.0f;
    private float rushSpeed = 9.0f;

    public BossEnemy mainDragon = null;

    public BoxCollider meleeArea = null;
    public BoxCollider rushArea = null;
    private Rigidbody rigid = null;
    private NavMeshAgent nav = null;
    private Animator ani = null;
    private EnemySpawn spawn = null;
    private CanvasUI canvasUI = null;

    // 몬스터의 공격 여부
    public bool isAttack = false;
    // 몬스터의 돌격 공격 여부
    private bool isRushAttack = false;
    // 몬스터의 돌격 공격 딜레이 
    private bool isDelay = false;
    private bool isBack = false;
    private bool isForward = false;
    // 본체 몬스터 존재 유무
    private bool isAlive = true;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        spawn = FindObjectOfType<EnemySpawn>();
        canvasUI = FindObjectOfType<CanvasUI>();
        mainDragon = FindObjectOfType<BossEnemy>();
    }

    private void Update()
    {
        if (!GameManager.Inst.MainPlayer.isDeath)
        {
            MainDragonDie();
            MainDragonReturn();
            RushAttackDelay();

            if (nav.enabled)
            {
                isDelay = true;
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

    // 되돌아갈 스폰 포인트 위치 저장.
    public void Init(EnemySpawn s) { spawn = s; }

    private void Targetting()
    {
        float targetRadius = 1.5f;
        float targetRange = 2.0f;

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, targetRadius,
                                    transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack && !isRushAttack)
        {
            StartCoroutine(Attack());
        }
    }

    // 몬스터 기본 공격
    IEnumerator Attack()
    {
        // 공격 시 플레이어를 바라보며 공격
        transform.LookAt(GameManager.Inst.MainPlayer.transform);

        yield return new WaitForSeconds(0.5f);
        isAttack = true;
        nav.enabled = false;
        ani.SetBool("isAttack", true);

        yield return new WaitForSeconds(3.0f);
        ani.SetBool("isAttack", false);
        nav.enabled = true;
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

    // 몬스터 돌격 공격
    private void RushAttackDelay()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 back = transform.TransformDirection(Vector3.back);

        // 돌격 공격 딜레이 true 일 때만 돌격 공격 패턴 실행
        if (isDelay)
        {
            // rushAttack에 시간을 더함
            rushAttack += Time.deltaTime;
            // rushAttack 의 시간이 딜레이보다 커지, 공격 중이 아닐 때 돌격 공격 코루틴 실행
            if (rushAttack > rushAttackDelay && !isAttack)
            {
                StartCoroutine(RushAttack());
            }
        }

        if (isBack)
        {
            transform.position += back * backSpped * Time.deltaTime;
        }

        if (isForward)
        {
            transform.position += forward * rushSpeed * Time.deltaTime;
        }
    }

    IEnumerator RushAttack()
    {
        rushAttack = 0.0f;
        isAttack = true;
        // 돌격공격 중 플레이어가 감지되어 바로 기본 공격 실행을 막기 위한 bool
        isRushAttack = true;
        nav.enabled = false;
        yield return new WaitForSeconds(0.5f);

        ani.SetBool("isRushBack", true);
        isBack = true;
        // 돌격 공격을 위한 뒷걸음질 시간
        yield return new WaitForSeconds(2.0f);
        isBack = false;
        isForward = true;
        ani.SetBool("isRushBack", false);
        ani.SetBool("isRushAttack", true);
        rushArea.enabled = true;
        // 돌격 공격 시간
        yield return new WaitForSeconds(3.1f);
        isForward = false;
        ani.SetBool("isRushAttack", false);
        rushArea.enabled = false;
        nav.enabled = true;
        isRushAttack = false;
        isAttack = false;
    }

    // 몬스터 피격 판정
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            GameManager.Inst.MainPlayer.enemyHitClip.Play();
            Sword weapon = other.GetComponent<Sword>();
            curHealth -= weapon.damage;

            if (curHealth <= 0)
            {
                OnDie();
            }
        }
    }

    // 본체 드래곤 사망 시 
    private void MainDragonDie()
    {
        if(mainDragon.isDie && isAlive)
        {
            isAlive = false;
            OnDie();
        }
    }

    // 본체 드래곤 스폰 포인트 복귀 시
    private void MainDragonReturn()
    {
        if(mainDragon.isOut == true)
        {
            Destroy(gameObject);
            canvasUI.AlterDragonHpBarOff();
        }
    }

    //플레이어 사망 시
    private void TargetDeath()
    {
        if (GameManager.Inst.MainPlayer.isDeath)
        {
            nav.enabled = false;
            ani.SetBool("isWalk", false);
            ani.SetTrigger("onJump");
        }
    }

    // 몬스터 처치 시 발생 함수
    private void OnDie()
    {
        canvasUI.AlterDragonHpBarOff();
        nav.enabled = false;
        ani.SetTrigger("onDie");
        Destroy(gameObject, 2.5f);
    }
}

