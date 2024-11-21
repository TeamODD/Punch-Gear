using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using TMPro;

namespace PunchGear
{
    public class EnemyPatten : MonoBehaviour
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
        public float term = 0.5f;

        private void Start()
        {
            StartCoroutine(patten());
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
            Instantiate(bullet, spawnPosition.transform.position, Quaternion.identity);
            //대충 소환
            yield return new WaitForSeconds(speed); // 시간 지연
        }

        IEnumerator patten()
        {
            while (true)
            {
                int randomInt = Random.Range(0, 5);

                switch(randomInt)
                {
                    case 0: yield return patten1(); break;
                    case 1: yield return patten2(); break;
                    case 2: yield return patten3(); break;
                    case 3: yield return patten4(); break;
                    case 4: yield return patten5(); break;
                }
                yield return new WaitForSeconds(term);
            }
        }

        IEnumerator patten1() // 패턴 1호기
        {
            yield return StartCoroutine(launch(normal));
            yield return StartCoroutine(transPos());
            yield return StartCoroutine(launch(normal));
            yield return StartCoroutine(transPos());
            yield return StartCoroutine(launch(normal));
        }

        IEnumerator patten2() // 패턴 2호기
        {
            yield return StartCoroutine(launch(fast));
            yield return StartCoroutine(launch(fast));
            yield return StartCoroutine(launch(fast));
            yield return StartCoroutine(launch(fast));
            yield return StartCoroutine(transPos());
            yield return StartCoroutine(launch(slow));
            yield return StartCoroutine(launch(slow));
        }

        IEnumerator patten3() // 패턴 3호기
        {
            yield return StartCoroutine(launch(normal));
            yield return StartCoroutine(launch(fast));
        }

        IEnumerator patten4() // 패턴 4호기
        {
            yield return StartCoroutine(transPos());
            yield return StartCoroutine(launch(fast));
            yield return StartCoroutine(launch(normal));
        }

        IEnumerator patten5() // 패턴 5호기
        {
            for(int i = 0; i < 5; i++)
            {
                int randomInt = Random.Range(0, 4);

                switch (randomInt)
                {
                    case 0: yield return transPos(); break;
                    case 1: yield return launch(slow); break;
                    case 2: yield return launch(normal); break;
                    case 3: yield return launch(fast); break;
                }
            }
        }
    }
}
