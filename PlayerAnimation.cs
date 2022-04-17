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
    //플레이어 속도파라메터를 애니메이터 컨트롤러에 반영하는 함수
    //속도 변화가 있는 경우 사용
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
    //앉기 토글시 상태를 애니메이터 컨트롤러에 반영하는 함수
    //앉기 토글 사용시 사용
    public void CrouchingDown()
    {
        animator.SetTrigger("Crouching/toggle");
    }
    //앉기 키 다운시 트리거를 애니메이터 컨트롤러에 반영하는 함수
    //앉기 키 다운 사용시 사용

    //앉기는 앉거나 일어설 때 충돌 판정으로 인해 일어설수 있는 곳인지 확인하고 사용할 수 있도록 코드 작성 요망
    public void Jump()
    {
        animator.SetTrigger("Jump");
    }
    //점프 시작을 애니메이터 컨트롤러에 반영하는 함수
    //점프 사용시 사용

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
    //플레이어가 지상에 있는지 공중에 있는지 상태를 애니메이터 컨트롤러에 반영하는 함수
    //플레이어가 지상에 있는지 확인할 때 사용
    //플레이어 컨트롤 클래스를 받아와서 if조건문을 직접 플레이어가 지상인지 아닌지 확인하는 조건문으로 바꾸는것을 추천

    //점프는 공중에서 다중 점프가 되지 않도록 코드 작성 요망
    public void Dead()
    {
        animator.SetTrigger("Dead");
    }
    //플레이어가 사망시 트리거를 애니메이터 컨트롤러에 반영하는 함수
    //플레이어가 사망시 사용
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
    //플레이어가 조준하려 할때 애니메이터 컨트롤러에 반영하는 함수
    //플레이어가 조준시 사용
    //플레이어가 조준을 할 수 있는 상황인지 확인하고 조준하게 코드 작성 요망
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
    //플레이어가 상호작용 할때 애니메이터 컨트롤러에 반영하는 함수
    //상호작용 시작시 한번, 상호작용 끝날 때 한번
    //플레이어가 상호작용시 사용
    public void Reload()
    {
        animator.SetTrigger("Reload");
    }
    //플레이어가 장전하려 할때 트리거를 애니메이터 컨트롤러에 반영하는 함수
    //플레이어가 장전시 사용
}
