using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace PunchGear.Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        public Internal.GameManager totalManager;
        private float HealthBarLength; // 최대 채력 길이

        private float scaleControl; // 이건 지금부터 스케일 조정기여
        private float positionControl; // 너는 지금부터 위치 조정기여
        private Vector3 positionVelocity = Vector3.zero; // 위치에 대한 속도 추적 변수
        private Vector3 scaleVelocity = Vector3.zero;    // 스케일에 대한 속도 추적 변수
        public float duration = 0.6f;

        private void Start()
        {
            HealthBarLength = this.transform.localScale.x;

            scaleControl = (float)HealthBarLength / totalManager.enemyHealthMax;
            positionControl = scaleControl * 0.5f;

            StartCoroutine(HealthHandler());
        }

        public IEnumerator HealthHandler()
        {
            Vector3 targetScale = transform.localScale - new Vector3(scaleControl, 0, 0);
            Vector3 targetPosition = transform.position - new Vector3(positionControl, 0, 0);

            float elapsedTime = 0f;
            while(elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                // SmoothDamp를 통해 부드럽게 이동
                transform.position = Vector3.SmoothDamp(
                    this.transform.position,
                    targetPosition,
                    ref positionVelocity,
                    0.2f // 감속 시간
                );

                transform.localScale = Vector3.SmoothDamp(
                    this.transform.localScale,
                    targetScale,
                    ref scaleVelocity,
                    0.2f // 감속 시간
                );

                elapsedTime += Time.deltaTime; // 경과 시간 증가
                yield return null; // 다음 프레임까지 대기
            }

            transform.localScale = targetScale;
            transform.localPosition = targetPosition;
        }
    }
}
