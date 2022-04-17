using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{
    [SerializeField]
    private GameObject attakcObject;
    [SerializeField]
    private Transform projectileSpawnPoint;
    [SerializeField]
    private bool bulletLine = false;

    private Animator animator;
    private EnemyFSM enemyFSM;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyFSM = GetComponentInParent<EnemyFSM>();
    }

    public float MoveSpeed
    {
        set
        {
            if (animator) animator.SetFloat("MoveState", value);
        }
        get
        {
            if (animator) return animator.GetFloat("MoveState");
            else return -1;
        }
    }

    public bool IsBattle
    {
        set
        {
            if (animator) animator.SetBool("isBattle", value);
        }
        get
        {
            if (animator) return animator.GetBool("isBattle");
            else return false;
        }
    }

    public float HitState
    {
        set => animator.SetFloat("HitState", value);
        get => animator.GetFloat("HitState");
    }
    public GameObject GameObject 
    {
        set => attakcObject = value;
        get => attakcObject; 
    }

    public void OnAttack()
    {
        if (bulletLine)
        {
            StartCoroutine("OnBulletLine");
        }
        else
        {
            animator.SetTrigger("OnAttack");
        }
    }

    public void Play(string stateName, int layer, float normalizedTime)
    {
        animator.Play(stateName, layer, normalizedTime);
    }

    public void OnAttackCollision()
    {
        if (attakcObject == null) return;

        attakcObject.SetActive(true);
    }

    public void OnSubject01_PorjectilePrefab()
    {
        if (attakcObject == null) return;

        GameObject clone = Instantiate(attakcObject, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        clone.GetComponent<Subject01_Porjectile>().SetUp(enemyFSM.Target.position);
    }

    private IEnumerator OnBulletLine()
    {
        if (attakcObject == null) yield break;

        attakcObject.GetComponent<EnemyArController>().BulletLine(enemyFSM.Target.position);

        yield return new WaitForSeconds(3f);

        animator.SetTrigger("OnAttack");
    }
    public void OnShootWithRay()
    {
        if (attakcObject == null) return;

        attakcObject.GetComponent<EnemyArController>().SetUp();
    }
    public void OnReload()
    {
        animator.SetTrigger("OnReload");
    }
    public void OnHit()
    {
        animator.SetTrigger("OnHit");
    }

    public bool EndState(string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >=1f) return true;
            return false;
        }
        else return false;
    }
}
