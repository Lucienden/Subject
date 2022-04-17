using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
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
    //�÷��̾� �ӵ��Ķ���͸� �ִϸ����� ��Ʈ�ѷ��� �ݿ��ϴ� �Լ�
    //�ӵ� ��ȭ�� �ִ� ��� ���
    public void CrouchingToggle()
    {
        if (animator.GetBool("Crouching/toggle") == false)
        {
            animator.SetBool("Crouching/toggle", true);
        }
        else
        {
            animator.SetBool("Crouching/toggle", false);
        }
    }
    //�ɱ� ��۽� ���¸� �ִϸ����� ��Ʈ�ѷ��� �ݿ��ϴ� �Լ�
    //�ɱ� ��� ���� ���
    public void CrouchingDown()
    {
        animator.SetTrigger("Crouching/toggle");
    }
    //�ɱ� Ű �ٿ�� Ʈ���Ÿ� �ִϸ����� ��Ʈ�ѷ��� �ݿ��ϴ� �Լ�
    //�ɱ� Ű �ٿ� ���� ���

    //�ɱ�� �ɰų� �Ͼ �� �浹 �������� ���� �Ͼ�� �ִ� ������ Ȯ���ϰ� ����� �� �ֵ��� �ڵ� �ۼ� ���
    public void Jump()
    {
        animator.SetTrigger("Jump");
    }
    //���� ������ �ִϸ����� ��Ʈ�ѷ��� �ݿ��ϴ� �Լ�
    //���� ���� ���

    public void Ground()
    {
        if(animator.GetBool("Ground") == false)
        {
            animator.SetBool("Ground", true);
        }
        else
        {
            animator.SetBool("Ground", false);
        }
    }
    //�÷��̾ ���� �ִ��� ���߿� �ִ��� ���¸� �ִϸ����� ��Ʈ�ѷ��� �ݿ��ϴ� �Լ�
    //�÷��̾ ���� �ִ��� Ȯ���� �� ���
    //�÷��̾� ��Ʈ�� Ŭ������ �޾ƿͼ� if���ǹ��� ���� �÷��̾ �������� �ƴ��� Ȯ���ϴ� ���ǹ����� �ٲٴ°��� ��õ

    //������ ���߿��� ���� ������ ���� �ʵ��� �ڵ� �ۼ� ���
    public void Dead()
    {
        animator.SetTrigger("Dead");
    }
    //�÷��̾ ����� Ʈ���Ÿ� �ִϸ����� ��Ʈ�ѷ��� �ݿ��ϴ� �Լ�
    //�÷��̾ ����� ���
    public void Aim()
    {
        if (animator.GetBool("Aim") == false)
        {
            animator.SetBool("Aim", true);
        }
        else
        {
            animator.SetBool("Aim", false);
        }
    }
    //�÷��̾ �����Ϸ� �Ҷ� �ִϸ����� ��Ʈ�ѷ��� �ݿ��ϴ� �Լ�
    //�÷��̾ ���ؽ� ���
    //�÷��̾ ������ �� �� �ִ� ��Ȳ���� Ȯ���ϰ� �����ϰ� �ڵ� �ۼ� ���
    public void Interaction()
    {
        if (animator.GetBool("Interaction") == false)
        {
            animator.SetBool("Interaction", true);
        }
        else
        {
            animator.SetBool("Interaction", false);
        }
    }
    //�÷��̾ ��ȣ�ۿ� �Ҷ� �ִϸ����� ��Ʈ�ѷ��� �ݿ��ϴ� �Լ�
    //��ȣ�ۿ� ���۽� �ѹ�, ��ȣ�ۿ� ���� �� �ѹ�
    //�÷��̾ ��ȣ�ۿ�� ���
    public void Reload()
    {
        animator.SetTrigger("Reload");
    }
    //�÷��̾ �����Ϸ� �Ҷ� Ʈ���Ÿ� �ִϸ����� ��Ʈ�ѷ��� �ݿ��ϴ� �Լ�
    //�÷��̾ ������ ���
}
