using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using PunchGear.Entity;

namespace PunchGear.Enemy
{
    public class EnemyPattern : MonoBehaviour
    {
        public GameObject bullet;
        public GameObject spawnPosition;

        public int height = 4; // 위아래 위치, 임시 세팅

        private bool _isMoving = false;
        private Coroutine _attackCoroutine;

        // 여기서 속도 수동 조작 가능
        public float slow = 1.2f;
        public float normal = 1.0f;
        public float fast = 0.8f;
        public float duration = 1f;
        public float term = 1.5f;
        public float smoothTime = 0.2f;

        private EntityPosition _position;

        private readonly List<IAttackPattern> _attackPatterns = new List<IAttackPattern>();

        private void Awake()
        {
            _attackPatterns.Add(new AttackPattern1(this, normal));
            _attackPatterns.Add(new AttackPattern2(this, fast, slow));
            _attackPatterns.Add(new AttackPattern3(this, normal, fast));
            _attackPatterns.Add(new AttackPattern4(this, normal, fast));
            _attackPatterns.Add(new AttackPattern5(this, slow, normal, fast));
        }

        private void Start()
        {
            _position = EntityPosition.Bottom;
            EntityPositionHandler.Instance.SetPosition(this, EntityPosition.Bottom);
            _attackCoroutine = StartCoroutine(Pattern());
        }

        private void OnDisable()
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
            }
        }

        public IEnumerator MoveOppositePosition() // 위치 반전 기계 에디션
        {
            _isMoving = true; // 이동 시작
            EntityPosition targetPosition = _position switch
            {
                EntityPosition.Bottom => EntityPosition.Top,
                EntityPosition.Top => EntityPosition.Bottom,
                _ => throw new InvalidOperationException("Undefined value")
            };
            Vector2 targetVector = EntityPositionHandler.Instance[targetPosition].Vector;
            targetVector.x = transform.position.x;
            float elapsedTime = 0f;
            Vector2 velocityVector = Vector2.zero;
            while (elapsedTime < duration)
            {
                // SmoothDamp를 통해 부드럽게 이동
                transform.position = Vector2.SmoothDamp(
                    transform.position,
                    targetVector,
                    ref velocityVector,
                    smoothTime // 감속 시간
                );
                elapsedTime += Time.deltaTime; // 경과 시간 증가
                yield return null; // 다음 프레임까지 대기
            }

            // 이동 완료 후 정확히 목표 위치로 설정
            EntityPositionHandler.Instance.SetPosition(this, targetPosition);
            _position = targetPosition;
            _isMoving = false;
        }

        public IEnumerator Launch(float speed)
        {
            GameObject bulletObject = Instantiate(bullet, spawnPosition.transform.position, Quaternion.identity);
            Projectile projectile = bulletObject.GetComponent<Projectile>();
            projectile.Position = _position;
            yield return new WaitForSeconds(speed); // 시간 지연
        }

        IEnumerator Pattern()
        {
            while (true)
            {
                int randomInt = UnityEngine.Random.Range(0, _attackPatterns.Count);
                IAttackPattern attackPattern = _attackPatterns[randomInt];
                yield return attackPattern.GetPatternCoroutine();
                yield return new WaitForSeconds(term);
            }
        }
    }
}
