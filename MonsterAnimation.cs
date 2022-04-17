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
    //몬스터 속도파라메터를 애니메이터 컨트롤러에 반영하는 함수
    //속도 변화가 있는 경우 사용
    public void Attack()
    {
        animator.SetTrigger("Attack");
    }
    //몬스터가 공격 할때 애니메이터 컨트롤러에 반영하는 함수
    //몬스터가 공격시 사용
    public void Dead()
    {
        animator.SetTrigger("Dead");
    }
    //몬스터가 사망 할때 애니메이터 컨트롤러에 반영하는 함수
    //몬스터가 사망시 사용
    public void Hit()
    {
        animator.SetTrigger("Hit");
    }
    //몬스터가 피격 당할때 애니메이터 컨트롤러에 반영하는 함수
    //몬스터가 피격시 사용
}
