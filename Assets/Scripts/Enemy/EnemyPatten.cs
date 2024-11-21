using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PunchGear.Enemy
{
    public class EnemyPattern : MonoBehaviour
    {
        public GameObject bullet;
        public GameObject spawnPosition;
        private Vector3 vel = Vector3.zero;
        public int height = 4; // 위아래 위치, 임시 세팅
        private bool pos = true;
        private bool isMoving = false;

        // 여기서 속도 수동 조작 가능
        public float slow = 1.2f;
        public float normal = 1.0f;
        public float fast = 0.8f;
        public float duration = 1f;
        public float term = 1.5f;

        private List<IAttackPattern> _attackPatterns = new List<IAttackPattern>();

        private void Awake()
        {
            _attackPatterns.Add(new AttackPattern1(this));
            _attackPatterns.Add(new AttackPattern2(this));
            _attackPatterns.Add(new AttackPattern3(this));
            _attackPatterns.Add(new AttackPattern4(this));
            _attackPatterns.Add(new AttackPattern5(this));
        }

        private void Start()
        {
            StartCoroutine(Pattern());
        }

        public IEnumerator TransPos() // 위치 반전 기계 에디션
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

        public IEnumerator Launch(float speed)
        {
            Instantiate(bullet, spawnPosition.transform.position, Quaternion.identity);
            //대충 소환
            yield return new WaitForSeconds(speed); // 시간 지연
        }

        IEnumerator Pattern()
        {
            while (true)
            {
                int randomInt = Random.Range(0, _attackPatterns.Count);
                IAttackPattern attackPattern = _attackPatterns[randomInt];
                yield return attackPattern.GetPatternCoroutine();
                yield return new WaitForSeconds(term);
            }
        }
    }
}
