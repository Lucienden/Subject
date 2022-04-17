using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyState { None = -1, Idle = 0, Wander, Pursuit, Attack, Hit, Die }

public class EnemyFSM : MonoBehaviour
{
    private GameObject targetObj;
    private Transform target;
    private LayerMask targetMask;
    private LayerMask exclusionMask;
    
    [Header("DamageText")]
    [SerializeField]
    private GameObject damageTextPrefab;
    [SerializeField]
    private Transform damageTextSpawPoint;

    private Vector3 destination = Vector3.zero;
    private float lastAttackTime;
    private EnemyState enemyState = EnemyState.None;

    private EnemyStatus enemyStatus;
    private NavMeshAgent navMeshAgent;
    private EnemyAnimatorController enemyAnimatorController;
    private AudioSource audioSource;
    private EnemyAudioController enemyAudioController;

    public Transform Target => target;
    public float AttackRange => enemyStatus.AttackRange;

    private void Awake()
    {
        targetObj = GameObject.FindGameObjectWithTag("Player");
        target = targetObj.transform;
        targetMask = LayerMask.GetMask("Player");
        exclusionMask = LayerMask.GetMask("Enemy");
        enemyStatus = GetComponent<EnemyStatus>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAnimatorController = GetComponentInChildren<EnemyAnimatorController>();
        audioSource = GetComponent<AudioSource>();
        enemyAudioController = GetComponent<EnemyAudioController>();

        navMeshAgent.updateRotation = false;
    }

    private void OnEnable()
    {
        ChangeState(EnemyState.Idle);
    }

    private void OnDisable()
    {
        StopCoroutine(enemyState.ToString());

        enemyState = EnemyState.None;
    }

    private void Update()
    {
        FieldOfView();    
    }

    private void FieldOfView()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, (enemyStatus.PursuitLimitRange-0.5f), targetMask);
        int index = 0;
        
        while (index < colls.Length)
        {
            Transform targetTf = colls[index].transform;

            if (targetTf.CompareTag("Player"))
            {
                Vector3 direction = (targetTf.position - transform.position).normalized;
                float angle = Vector3.Angle(direction, transform.forward);

                if (angle <= enemyStatus.ViewAngle * 0.5f)
                {
                    Ray ray = new Ray(transform.position + transform.up, direction);
                    RaycastHit hit;
                    
                    if(Physics.Raycast(ray, out hit, enemyStatus.PursuitLimitRange - 0.5f, ~exclusionMask))
                    {
                        Debug.Log("보고 있는 트랜스폼: " + hit.transform.name);
                        if(hit.transform.CompareTag("Player") && !enemyAnimatorController.IsBattle)
                        {
                            ChangeState(EnemyState.Pursuit);
                        }
                    }
                }
            }
            index++;
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (enemyState == newState) return;

        StopCoroutine(enemyState.ToString());

        enemyState = newState;

        StartCoroutine(enemyState.ToString());
    }

    private IEnumerator Idle()
    {
        navMeshAgent.ResetPath();
        enemyAnimatorController.IsBattle = false;
        PlayClip(enemyAudioController.IdleClip);

        StartCoroutine("AutoChangeFromIdleToWander");

        while (true)
        {
            if (enemyAnimatorController.MoveSpeed > 0f) enemyAnimatorController.MoveSpeed -= 0.01f;
            else enemyAnimatorController.MoveSpeed = 0f;

            CalculateDistanceToTragetAndSelectState();

            yield return null;
        }
    }

    private IEnumerator AutoChangeFromIdleToWander()
    {
        int changeTime = Random.Range(1, 3);

        destination = CalculateWanderPosition();

        yield return new WaitForSeconds(changeTime);

        ChangeState(EnemyState.Wander);
    }

    private bool LockWanderRotation()
    {
        Vector3 to = new Vector3(destination.x, 0f, destination.z);
        Vector3 from = new Vector3(transform.position.x, 0f, transform.position.z);

        Vector3 direction = (to - from).normalized;
        float angle = Vector3.Angle(direction, transform.position);

        if (angle <= 0.1f) return true;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(to - from), 1.5f);
        return false;
    }

    private IEnumerator Wander()
    {
        PlayClip(enemyAudioController.WalkClip);

        float currentTime = 0;
        float maxTime = 10;

        Vector3 to = new Vector3(destination.x, 0f, destination.z);
        Vector3 from;

        navMeshAgent.speed = enemyStatus.WalkSpeed;
        navMeshAgent.SetDestination(destination);

        while (true)
        {
            currentTime += Time.deltaTime;
            from = new Vector3(transform.position.x, 0f, transform.position.z);

            if (enemyAnimatorController.MoveSpeed < 0.5f) enemyAnimatorController.MoveSpeed += 0.02f;
            else enemyAnimatorController.MoveSpeed = 0.5f;

            LockWanderRotation();

            if ((to-from).sqrMagnitude < 0.01f || currentTime >= maxTime)
            {
                ChangeState(EnemyState.Idle);
            }

            CalculateDistanceToTragetAndSelectState();

            yield return null;
        }
    }
    
    private Vector3 CalculateWanderPosition()
    {
        float wanderRadius = 10f;
        int wanderJitter = 0;
        int wanderJitterMin = 0;
        int wanderJitterMax = 360;

        Vector3 rangePosition = Vector3.zero;
        Vector3 rangeScale = Vector3.one * 100f;

        wanderJitter = Random.Range(wanderJitterMin, wanderJitterMax);
        Vector3 targetPosition = transform.position + SetAngle(wanderRadius, wanderJitter);

        targetPosition.x = Mathf.Clamp(targetPosition.x, rangePosition.x - rangeScale.x * 0.5f, rangePosition.x + rangeScale.x * 0.5f);
        targetPosition.y = 0f;
        targetPosition.z = Mathf.Clamp(targetPosition.z, rangePosition.z - rangeScale.z * 0.5f, rangePosition.z + rangeScale.z * 0.5f);

        return targetPosition;
    }

    private Vector3 SetAngle(float radius, int angle)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Cos(angle) * radius;
        position.z = Mathf.Sin(angle) * radius;

        return position;
    }

    private IEnumerator Pursuit()
    {
        navMeshAgent.speed = enemyStatus.RunSpeed;
        enemyAnimatorController.IsBattle = false;

        PlayClip(enemyAudioController.RunClip);

        while (true)
        {
            if (enemyAnimatorController.MoveSpeed < 1f) enemyAnimatorController.MoveSpeed += 0.1f;
            else enemyAnimatorController.MoveSpeed = 1f;
            navMeshAgent.SetDestination(target.position);

            LockTargetRotation();

            CalculateDistanceToTragetAndSelectState();

            yield return null;
        }
    }

    private bool LockTargetRotation()
    {
        destination = new Vector3(target.position.x, 0, target.position.z);
      
        Vector3 to = new Vector3(destination.x, 0f, destination.z);
        Vector3 from = new Vector3(transform.position.x, 0f, transform.position.z);

        Vector3 direction = (to - from).normalized;
        float angle = Vector3.Angle(direction, transform.position);

        if (angle <= 0.1f) return true;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(to - from), enemyStatus.SpinSpeed);
        return false;
    }

    private void CalculateDistanceToTragetAndSelectState()
    {
        if (target == null) return;
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= enemyStatus.AttackRange)
        {
            ChangeState(EnemyState.Attack);
            return;
        }
        else if (distance <= enemyStatus.TargetRecognitionRange)
        {
            ChangeState(EnemyState.Pursuit);
        }
        else if (distance > enemyStatus.PursuitLimitRange && enemyState == EnemyState.Pursuit)
        {
            navMeshAgent.ResetPath();
            ChangeState(EnemyState.Idle);
        }
    }

    private IEnumerator Attack()
    {
        navMeshAgent.ResetPath();

        enemyAnimatorController.IsBattle = true;

        while (true)
        {
            if (enemyAnimatorController.MoveSpeed > 0f) enemyAnimatorController.MoveSpeed -= 0.1f;
            else enemyAnimatorController.MoveSpeed = 0f;

            if (Time.time - lastAttackTime > enemyStatus.AttackRate)
            {
                lastAttackTime = Time.time;

                enemyAnimatorController.OnAttack();

                PlayClip(enemyAudioController.AttackClip);
            }

            LockTargetRotation();

            CalculateDistanceToTragetAndSelectState();

            yield return null;

        }
    }

    private void PlayClip(AudioClip newClip)
    {
        if (newClip == null) return;
        if (audioSource.clip == newClip) return;
        
        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();
        audioSource.loop = true;
    }

    public void TakeDamage(float damage) // 플레이어 공격 trigger에 들어갈 시, 발동
    {
        if (enemyStatus.HealthPoint <= 0) return;

        StopAllCoroutines();

        GameObject cloneDmgText = Instantiate(damageTextPrefab, damageTextSpawPoint.position, target.rotation);
        cloneDmgText.GetComponent<DamageText>().SetUp(damage);

        enemyStatus.HealthPoint -= damage;

        if (enemyStatus.HealthPoint <= 0)
        {
            enemyStatus.HealthPoint = 0;
            ChangeState(EnemyState.Die);
        }
        else
        {
            if (enemyState == EnemyState.Hit) ReHit();
            ChangeState(EnemyState.Hit);
        }
    }

    private IEnumerator Die()
    {
        navMeshAgent.isStopped = true;

        enemyAnimatorController.Play("Die", -1, 0);

        yield return new WaitForSeconds(2f);

        GetComponentInParent<SpawnerController>().OnDeactivateItem(gameObject);
    }

    private IEnumerator Hit()
    {
        navMeshAgent.ResetPath();

        float random = Random.Range(0, 2) / 1;
        enemyAnimatorController.HitState = random;
        enemyAnimatorController.OnHit();

        if (target == null) ChangeState(EnemyState.Idle);
        if (enemyAnimatorController.IsBattle == true) ChangeState(EnemyState.Attack);

        float currentTime = 0;
        float maxTime = 10;

        Vector3 point = target.position;
        Vector3 to = new Vector3(point.x, 0f, point.z);
        Vector3 from;

        navMeshAgent.speed = enemyStatus.RunSpeed;
        navMeshAgent.SetDestination(point);

        PlayClip(enemyAudioController.RunClip);

        while (true)
        {
            from = new Vector3(transform.position.x, 0f, transform.position.z);
            if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(to - from)) <= 1f)
            {
                transform.rotation = Quaternion.LookRotation(to - from);
                break;
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(to - from), enemyStatus.SpinSpeed);

            yield return null;
        }

        while (true)
        {
            if (enemyAnimatorController.MoveSpeed < 1f) enemyAnimatorController.MoveSpeed += 0.01f;
            else enemyAnimatorController.MoveSpeed = 1f;

            currentTime += Time.deltaTime;
            if ((point - transform.position).sqrMagnitude <= 5f || currentTime >= maxTime)
            {
                ChangeState(EnemyState.Idle);
            }

            CalculateDistanceToTragetAndSelectState();

            yield return null;
        }
    }
    private void ReHit()
    {
        StopCoroutine("Hit");
        StartCoroutine("Hit");
    }

    private void OnDrawGizmos()
    {
        if (navMeshAgent == null) return;

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, navMeshAgent.destination - transform.position);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyStatus.TargetRecognitionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, enemyStatus.PursuitLimitRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, enemyStatus.AttackRange);
    }
}
