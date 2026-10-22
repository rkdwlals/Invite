using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EthanTheHero
{
    public class PlayerAnimation : MonoBehaviour
    {
        #region FIELD
        private PlayerMovement playerMv; // PlayerMovement 컴포넌트 위한 변수
        private Animator myAnim; // Animator 컴포넌트 위한 변수
        private Rigidbody2D myBody; // Rigidbody2D 위한 변수

        // 애니메이션의 매개변수 이름 정의
        private const string speed = "Speed"; // 이동 속도
        private const string runIdle = "RunIdlePlayying"; // 대기 중 애니메이션 재생
        private const string jump = "Grounded"; // 점프
        private const string yvelocity = "Yvelocity"; // Y축 속도
        private const string dash = "Dashing"; // 대시
        private const string wallSliding = "WallSliding"; // 벽 슬라이딩
        private const string wallJump = "WallJump"; // 벽 점프
        private const string hurt = "Hurt"; // 피해.
        private const string hurtEnded = "HurtEnded"; // 피해 끝
        private const string death = "Death"; // 죽음
        private const string deathEnded = "DeathEnded"; // 죽음 끝

        private bool runIdleIsPlayying; // 애니메이션 재생 체크하는 거

        #endregion

        void Awake()
        {
            // 컴포넌트를 초기화.
            playerMv = GetComponent<PlayerMovement>(); // PlayerMovement 컴포넌트
            myAnim = GetComponent<Animator>(); // Animator 컴포넌트
            myBody = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트
        }

        void Update()
        {
            #region IDLE & RUN

            // 플레이어의 이동 속도 전달.
            myAnim.SetFloat(speed, Mathf.Abs(playerMv.move.x));

            // 애니메이션이 "RunIdleTrans"일 때 대기 중 애니메이션 재생 체크
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("RunIdleTrans"))
            {
                runIdleIsPlayying = true; // 대기 중 애니메이션 재생 중으로 설정
                // 애니메이션이 종료되면 대기 중 애니메이션 재생 여부 false로 설정
                if (myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                    runIdleIsPlayying = false;
            }
            // 대기 중 애니메이션 재생 여부 애니메이터에 전달
            myAnim.SetBool(runIdle, runIdleIsPlayying);

            #endregion
        }
    }
}



