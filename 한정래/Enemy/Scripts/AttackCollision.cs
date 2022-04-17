using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    private EnemyStatus enemyStatus;

    private void Awake()
    {
        enemyStatus = GetComponentInParent<EnemyStatus>();
    }
    private void OnEnable()
    {
        StartCoroutine("AutoDisable");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(enemyStatus.AttackDamage+"�� �������� �÷��̾ ����");
        }
    }

    private IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(1f);

        this.gameObject.SetActive(false);
    }
}
