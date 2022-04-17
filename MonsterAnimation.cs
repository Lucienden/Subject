using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimation : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Speed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }
    //���� �ӵ��Ķ���͸� �ִϸ����� ��Ʈ�ѷ��� �ݿ��ϴ� �Լ�
    //�ӵ� ��ȭ�� �ִ� ��� ���
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
    //���Ͱ� ���� �Ҷ� �ִϸ����� ��Ʈ�ѷ��� �ݿ��ϴ� �Լ�
    //���Ͱ� ���ݽ� ���
    public void Dead()
    {
        animator.SetTrigger("Dead");
    }
    //���Ͱ� ��� �Ҷ� �ִϸ����� ��Ʈ�ѷ��� �ݿ��ϴ� �Լ�
    //���Ͱ� ����� ���
    public void Hit()
    {
        animator.SetTrigger("Hit");
    }
    //���Ͱ� �ǰ� ���Ҷ� �ִϸ����� ��Ʈ�ѷ��� �ݿ��ϴ� �Լ�
    //���Ͱ� �ǰݽ� ���
}
