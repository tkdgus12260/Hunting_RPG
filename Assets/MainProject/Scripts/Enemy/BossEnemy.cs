using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : MonoBehaviour
{

    public float curHealth = 2000.0f;
    public float maxHealth = 2000.0f;
    private float attackDis = 10.0f;
    private float returnDis = 60.0f;

    public float rushAttack = 0.0f;
    public float rushAttackDelay = 15.0f;
    private float backSpped = 3.0f;
    private float rushSpeed = 9.0f;
    public float chargingSkillDelay = 20.0f;
    private float sandstormDamage = 1000.0f;

    public GameObject AlterEgoDragon = null;
    public GameObject chargingSandstorm = null;
    private BossTreeEffect treeEffect = null;
    private BossTree tree = null;

    public BoxCollider meleeArea = null;
    public BoxCollider rushArea = null;
    private Rigidbody rigid = null;
    private NavMeshAgent nav = null;
    private Animator ani = null;
    private EnemySpawn spawn = null;
    private BossEnemyUI bossEnemyUI = null;
    private CanvasUI canvasUI = null;

    // 플레이어 인식 거리 밖으로 나왔을 때
    public bool isOut = false;
    public bool isDie = false;
    // 몬스터의 공격 여부
    public bool isAttack = false;
    // 몬스터의 돌격 공격 여부
    private bool isRushAttack = false;
    // 몬스터의 돌격 공격 딜레이 
    private bool isDelay = false;
    private bool isBack = false;
    private bool isForward = false;
    // 차징 스킬 여부
    private bool isChargingSkill = false;
    // 분신 스킬 여부
    private bool isAlterEgo = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        spawn = FindObjectOfType<EnemySpawn>();
        bossEnemyUI = FindObjectOfType<BossEnemyUI>();
        canvasUI = FindObjectOfType<CanvasUI>();
        treeEffect = FindObjectOfType<BossTreeEffect>();
        tree = FindObjectOfType<BossTree>();
    }

    private void Update()
    {
        if (!GameManager.Inst.MainPlayer.isDeath)
        {
            FollowTarget();
            RushAttackDelay();
            //ChargingAttack();
            StartCoroutine(AlterEgo());

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

    //되돌아갈 스폰 포인트 위치 저장.
    public void Init(EnemySpawn s) { spawn = s; }

    // 몬스터 플레이어 인식 함수
    private void FollowTarget()
    {
        float targetDistance = Vector3.Distance(transform.position, GameManager.Inst.MainPlayer.transform.position);
        float spawnDistance = Vector3.Distance(transform.position, spawn.transform.position);

        // 타겟이 인식 거리 내에 감지 되었을 때 실행
        if (targetDistance < attackDis && !isOut && !isDie && !isAttack)
        {
            nav.enabled = true;
            canvasUI.DragonHpBarOn();
        }

        if (spawnDistance > returnDis)
        {
            // 몬스터 어그로 해제 시 체력 회복
            curHealth = maxHealth;
            isOut = true;
            nav.enabled = false;
        }
        // 스폰 포인트 도착 시
        else if (spawnDistance < 0.5f)
        {
            transform.LookAt(GameManager.Inst.MainPlayer.transform);
            isOut = false;
            ani.SetBool("isWalk", false);
            canvasUI.DragonHpBarOff();
        }

        if (isOut == true)
        {
            isDelay = false;
            transform.LookAt(spawn.gameObject.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, spawn.transform.position, 5 * Time.deltaTime);
        }
    }

    // 몬스터 플레이어 공격 인식 함수
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

    // 몬스터 차징 스킬
    private void ChargingAttack()
    {
        // 몬스터의 체력이 절반 이하로 되면 차징 공격 패턴 실행
        if(curHealth <= maxHealth / 2 && !isChargingSkill)
        {
            isDelay = false;
            isAttack = true;
            nav.enabled = false;
            ani.SetBool("isCharging", true);

            chargingSkillDelay -= Time.deltaTime;

            // BossTree의 bool값으로 이펙트의 온오프 여부 확인 후 결정/ 이펙트가 켜지면 더이상 TreeEffect를 찾지 않음.
            if (!tree.isEffectOn)
            {
                GameObject.Find("ChargingTree").transform.Find("TreeEffect").gameObject.SetActive(true);
            }

            // 차징 스킬 패턴 파훼 성공
            if (tree.isDestroy)
            {
                isChargingSkill = true;
                isDelay = true;
                isAttack = false;
                nav.enabled = true;
                ani.SetBool("isCharging", false);
            }

            // 차징 스킬 패턴 파훼 실패
            if (chargingSkillDelay < 0)
            {
                StartCoroutine(Sandstorm());
                isDelay = true;
                isAttack = false;
                nav.enabled = true;
                isChargingSkill = true;
                // 패턴 파훼 실패 시 나무 파괴
                tree.FailPattern();
                ani.SetBool("isCharging", false);
            }

        }
    }

    // 차징 스킬 파훼 실패 시 패턴
    IEnumerator Sandstorm()
    {
        chargingSandstorm.SetActive(true);
        GameManager.Inst.MainPlayer.TakeDamage(sandstormDamage);
        yield return new WaitForSeconds(5.0f);

        chargingSandstorm.SetActive(false);
    }

    // 몬스터 분신 스킬 코루틴
    IEnumerator AlterEgo()
    {
        if(curHealth <= maxHealth / 4 && !isAlterEgo)
        {
            canvasUI.AlterDragonHpBarOn();
            isAlterEgo = true;
            ani.SetTrigger("onScream");
            isAttack = true;
            nav.enabled = false;
            Instantiate(AlterEgoDragon, transform.position, transform.rotation);
            yield return new WaitForSeconds(2.3f);
            isAttack = false;
            nav.enabled = true;
        }
    }

    private void TargetDeath()
    {
        if (GameManager.Inst.MainPlayer.isDeath)
        {
            nav.enabled = false;
            ani.SetBool("isWalk", false);
            ani.SetTrigger("onJump");
        }
    }

    // 몬스터 피격 판정
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Sword weapon = other.GetComponent<Sword>();
            curHealth -= weapon.damage;

            // 몬스터 사망 시
            if (curHealth <= 0)
            {
                OnDie();
            }
        }
    }
    // 몬스터 처치 시 발생 함수
    private void OnDie()
    {
        isDie = true;
        nav.enabled = false;
        canvasUI.DragonHpBarOff();
        ani.SetTrigger("onDie");
        Destroy(gameObject, 2.5f);
    }
}
