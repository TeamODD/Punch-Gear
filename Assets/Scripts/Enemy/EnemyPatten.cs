using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using TMPro;

namespace PunchGear
{
    public class EnemyPatten : MonoBehaviour
    {
        private Vector3 vel = Vector3.zero;
        public int height = 4; // 위아래 위치, 임시 세팅
        private bool pos = true;
        private bool isMoving = false;

        // 여기서 속도 수동 조작 가능
        public float slow = 1.2f;
        public float normal = 1.0f;
        public float fast = 0.8f;
        public float duration = 1f;

        
        private void Update()
        {

        }

        public IEnumerator transPos() // 위치 반전 기계 에디션
        {
            isMoving = true; // 이동 시작

            Vector3 startPosition = this.transform.position; // 시작 위치
            Vector3 targetPosition = startPosition + new Vector3(0, height * (pos ? 1 : -1), 0); // 목표 위치

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;

                // SmoothDamp를 통해 부드럽게 이동
                this.transform.position = Vector3.SmoothDamp(
                    gameObject.transform.position,
                    targetPosition,
                    ref vel,
                    0.2f // 감속 시간
                );

                elapsedTime += Time.deltaTime; // 경과 시간 증가
                yield return null; // 다음 프레임까지 대기
            }

            // 이동 완료 후 정확히 목표 위치로 설정
            this.transform.position = targetPosition;

            pos = !pos; // 방향 반전
            isMoving = false; // 이동 완료
        }

        private IEnumerator launch(float speed)
        {
            //대충 소환
            yield return new WaitForSeconds(speed); // 시간 지연
        }

        // 발사(보통) > 방향 전환 > 발사(보통) > 방향 전환 > 발사(보통)

        // 이후 패턴 세분화
        // 패턴 1호기 위치 변경
        // 패턴 2호기 느리게 발사
        // 패턴 3호기 일반 발사
        // 패턴 4호기 빠르게 발사

        // 대충 네 개 섞어서 확률로 대충 돌리면 미친 패턴이 나오지 않을까
        // 패턴 텀은 2~4호기로 결정

        // 패턴이 여러개로 늘어나면 너무 두렵다..
        // 흚
        private void Patten1()
        {
            // 원래 자리(아래)
            for (int i = 0; i < 3; i++)
            {
                // 대충 소환

                // 위지 변환
                launch(slow);
                // 약간의 텀이 좀 필요
            } // 두려워진다. 내 코드를 보지말아줘!!!!

            // 부드러운 움직임 추가?
            // 위로 힘을 줘서, 특정 위치에 갈 때까지 이동? 감속과 가속? 등속? 흚
        }

        // 적 패턴 만들기

        // 패턴 1호기
        // 아래 위 아래 (느림)

        // 패턴 2호기
        // 아래 * 4 (빠르게)
        // 위 * 2

        // 패턴 3호기
        // 아래 * 2(보통)
    }
}
