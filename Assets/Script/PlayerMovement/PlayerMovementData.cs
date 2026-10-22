using UnityEngine;

namespace EthanTheHero
{
    [CreateAssetMenu(menuName = "Player Movement Data")]
    public class PlayerMovementData : ScriptableObject
    {
        [Header("Run")]
        public float runMaxSpeed; // 플레이어가 도달하고자 하는 목표 속도
        public float runAcceleration; // 0에서 runMaxSpeed에 도달하기까지의 대략적인 시간
        [HideInInspector] public float runAccelAmount; // 실제로 플레이어에게 적용되는 힘 (speedDiff와 곱해짐)
        public float runDecceleration; // runMaxSpeed에서 0으로 감속하기까지의 대략적인 시간
        [HideInInspector] public float runDeccelAmount; // 실제로 플레이어에게 적용되는 감속 힘 (speedDiff와 곱해짐)
        [Space(10)]
        [Range(0.01f, 1)] public float accelInAir; // 공중에서 가속률에 적용되는 배율
        [Range(0.01f, 1)] public float deccelInAir; // 공중에서 감속률에 적용되는 배율
        public bool doConserveMomentum; // 모멘텀 보존 여부

        [Space(20)]

        [Header("Jump")]
        public float jumpHeight; // 플레이어의 점프 높이

        [Space(20)]

        [Header("Dash")]
        public float dashPower = 30f; // 대시 시 힘
        public float dashingCoolDown = 1f; // 대시 쿨다운 시간
        public float dashingTime = 0.2f; // 대시 지속 시간

        [Space(20)]

        [Header("Wall Sliding and Wall Jumping")]
        public float wallDistance = 0.05f; // 벽 체크를 위한 거리
        [HideInInspector] public float wallJumpTime = 0.2f; // 벽 점프 지속 시간
        public float wallSlideSpeed = 0.3f; // 벽 슬라이딩 속도
        public float wallJumpingYPower = 6.5f; // 벽 점프 시 Y축 방향으로 가해지는 힘
        public float wallJumpingXPower = 5f; // 벽 점프 시 X축 방향으로 가해지는 힘
        public float WallJumpTimeInSecond = 0.1f; // 벽 점프 후 대기 시간

        // OnValidate는 인스펙터에서 값이 변경될 때 호출
        private void OnValidate()
        {
            // 가속량 및 감속량 계산
            runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
            runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

            #region Variable Ranges
            // 가속도 및 감속도 값 제한
            runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
            runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
            #endregion
        }
    }
}
