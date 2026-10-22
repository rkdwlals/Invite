using System.Collections;
using UnityEngine;

namespace EthanTheHero
{
    public class PlayerMovement : MonoBehaviour
    {
        #region FIELD

        [SerializeField] private PlayerMovementData data; // 플레이어 이동 데이터
        [SerializeField] private Transform WallCheck; // 벽 체크를 위한 위치
        [SerializeField] private LayerMask wallLayer; // 벽 레이어 설정

        [HideInInspector] public Vector2 move; // 현재 이동 방향

        private Rigidbody2D myBody; // 플레이어의 Rigidbody2D 컴포넌트
        private Animator myAnim; // 플레이어의 Animator 컴포넌트

        // 대시 관련 변수
        [HideInInspector] public bool isDashing; // 대시 중
        private bool canDash = true; // 대시 가능
        private bool dashButtonPressed; // 대시 버튼이 눌렸는지

        // 벽 슬라이딩 및 점프 관련 변수
        [HideInInspector] public bool wallJump; // 벽 점프
        [HideInInspector] public bool wallSliding; // 벽 슬라이딩
        private RaycastHit2D wall; // 벽 체크하기 위한 RaycastHit2D
        private float jumpTime; // 점프 시간

        #endregion

        #region MONOBEHAVIOUR
        void Awake()
        {
            myBody = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 초기화
            myAnim = GetComponent<Animator>(); // Animator 컴포넌트 초기화
            myBody.gravityScale = 0f; // 중력 비활성화
        }

        void Update()
        {
            // 대시 중, 벽 점프 중, 또는 공격 애니메이션 중이라면 입력을 무시
            if (isDashing || wallJump || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01") ||
                myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02") ||
                myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
                return;

            // WASD 입력 처리 (이동)
            move.x = Input.GetAxisRaw("Horizontal"); // A, D 키 입력
            move.y = Input.GetAxisRaw("Vertical");   // W, S 키 입력

            // 대시 키 변경 (이전 W에서 다른 키로 변경, 예: LeftShift)
            dashButtonPressed = Input.GetKeyDown(KeyCode.LeftShift); // LeftShift로 대시

            // 이동 방향 확인 (왼쪽 또는 오른쪽)
            if (move.x != 0)
                CheckDirectionToFace(move.x > 0);

            // 대시 버튼이 눌리면 대시 시작
            if (dashButtonPressed && canDash && !wallSliding)
                StartCoroutine(dash());
        }

        void FixedUpdate()
        {
            // 대시 중, 벽 점프 중, 또는 공격 애니메이션 중이라면 입력을 무시
            if (isDashing || wallJump || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack01") ||
                myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack02") ||
                myAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack03"))
                return;

            // 벽 슬라이딩이 아닐 경우 이동 처리
            if (!wallSliding)
            {
                run(); // 이동 함수 호출
            }

            WallSlidngMechanic(); // 벽 슬라이딩 메커니즘 처리
        }

        #endregion

        #region RUN
        private void run()
        {
            // 입력이 있을 때 이동 속도 설정
            if (move.x != 0 || move.y != 0)
            {
                // 캐릭터의 이동 속도를 입력에 따라 설정
                myBody.linearVelocity = new Vector2(move.x * data.runMaxSpeed, move.y * data.runMaxSpeed);
            }
            else
            {
                // 입력이 없을 때 속도를 0으로 설정하여 즉시 멈춤
                myBody.linearVelocity = Vector2.zero;
            }
        }

        #endregion

        #region DASH
        private IEnumerator dash()
        {
            canDash = false; // 대시 불가 상태로 설정
            isDashing = true; // 대시 상태로 설정
            float oriGrav = myBody.gravityScale; // 원래 중력 값 저장
            myBody.gravityScale = 0f; // 대시 중에는 중력 비활성화

            // 대시 방향에 따라 속도 설정
            myBody.linearVelocity = new Vector2(transform.localScale.x * data.dashPower, 0f);
            yield return new WaitForSeconds(data.dashingTime); // 대시 시간 동안 대기

            // 대시 후 원래 속도로 복귀
            if (move.x > 0)
            {
                myBody.linearVelocity = new Vector2(data.runMaxSpeed, myBody.linearVelocity.y);
            }
            else if (move.x < 0)
            {
                myBody.linearVelocity = new Vector2(-data.runMaxSpeed, myBody.linearVelocity.y);
            }

            myBody.gravityScale = oriGrav; // 원래 중력 값으로 복구

            isDashing = false; // 대시 상태 종료
            yield return new WaitForSeconds(data.dashingCoolDown); // 대시 쿨다운 시간 대기
            canDash = true; // 대시 가능 상태로 설정
        }
        #endregion

        #region Wall Sliding and Wall Jump
        private void WallSlidngMechanic()
        {
            // 벽 체크를 위한 레이캐스트
            wall = Physics2D.Raycast(WallCheck.position, new Vector2(data.wallDistance, 0f), data.wallDistance, wallLayer);
            Debug.DrawRay(WallCheck.position, new Vector2(data.wallDistance, 0f), Color.red); // 레이캐스트 시각화

            // 벽이 감지되면 슬라이딩 상태로 설정
            if (wall)
            {
                wallSliding = true;
                jumpTime = Time.time + data.wallJumpTime; // 점프 시간 초기화
            }
            else if (jumpTime < Time.time) // 점프 시간이 지나면 슬라이딩 상태 종료
                wallSliding = false;
            else
                wallSliding = false;

            // 슬라이딩 상태일 경우 수직 속도 제한
            if (wallSliding)
                myBody.linearVelocity = new Vector2(myBody.linearVelocity.x, Mathf.Clamp(myBody.linearVelocity.y, -data.wallSlideSpeed, float.MaxValue));
        }

        private IEnumerator wallJumpMechanic()
        {
            wallJump = true; // 벽 점프 상태로 설정
            // 벽의 방향에 따라 점프 속도 설정
            if (transform.localScale.x == -1f)
                myBody.linearVelocity = new Vector2(data.wallJumpingXPower, data.wallJumpingYPower);
            else
                myBody.linearVelocity = new Vector2(-data.wallJumpingXPower, data.wallJumpingYPower);
            yield return new WaitForSeconds(data.WallJumpTimeInSecond); // 점프 후 대기 시간
            wallJump = false; // 벽 점프 상태 종료
        }
        #endregion

        #region OTHER
        private void CheckDirectionToFace(bool isMovingRight)
        {
            Vector3 tem = transform.localScale; // 현재 스케일 저장
            if (!isMovingRight)
                tem.x = -1f; // 왼쪽으로 이동 시 방향 반전
            else
                tem.x = 1f; // 오른쪽으로 이동 시 방향 유지
            transform.localScale = tem; // 방향 반영
        }
        #endregion
    }
}
