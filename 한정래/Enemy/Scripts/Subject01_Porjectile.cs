using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject01_Porjectile : MonoBehaviour
{
    [SerializeField]
    private float projectileSpeed = 1f;
    [SerializeField]
    private float projectileDistance = 80f;
    [SerializeField]
    private float projectileDamage = 3f;
    [SerializeField]
    private GameObject collisionEffect;

    private Vector3 moveDirection = Vector3.zero;

    public void SetUp(Vector3 targetPosition)
    {
        StartCoroutine("OnMove", targetPosition);
    }

    private IEnumerator OnMove(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        moveDirection = (targetPosition - startPosition).normalized;

        while (true)
        {
            transform.position += moveDirection * projectileSpeed * Time.deltaTime;

            if(Vector3.Distance(transform.position, startPosition) >= projectileDistance)
            {
                Destroy(this.gameObject);
                yield break;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾��" + projectileDamage + "�� �������� ������.");

            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Debug.Log("���� �ε���");
            GameObject clone = Instantiate(collisionEffect, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}