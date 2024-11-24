using System;
using System.Collections;
using PunchGear.Enemy;
using Unity.VisualScripting;
using UnityEngine;

namespace PunchGear.Entity.UI
{
    public class NobilityHealthIndicator : MonoBehaviour
    {
        private EnemyObject _enemyObject;

        [SerializeField]
        private Transform _targetIndicator;
        [Range(0.01f, 1f)]
        [SerializeField]
        private float _indicatorShrinkRate;

        private Vector3 _originalScale;
        private int _originalNobilityHealth;

        private void Awake()
        {
            _enemyObject = FindFirstObjectByType<EnemyObject>();
            if (_enemyObject == null)
            {
                throw new NullReferenceException("Cannot find enemy object");
            }
            if (_targetIndicator == null)
            {
                throw new NullReferenceException("Indicator transform is not attached");
            }
            _originalScale = _targetIndicator.localScale;
        }

        private void Start()
        {
            _enemyObject.OnHealthChange += UpdateNobilityHeath;
            Initialize();
        }

        private void OnDisable()
        {
            _enemyObject.OnHealthChange -= UpdateNobilityHeath;
        }

        private void Initialize()
        {
            _originalNobilityHealth = _enemyObject.Health;
        }

        private void UpdateNobilityHeath(int previousHealth, int currentHealth)
        {
            if (currentHealth >= previousHealth || currentHealth < 0)
            {
                return;
            }
            StartIndicateShrink(currentHealth, _indicatorShrinkRate);
        }

        public Coroutine StartIndicateShrink(int health, float cooldown)
        {
            float indicatorLength = health / (float)_originalNobilityHealth;
            return StartCoroutine(StartShrinkIndicator(indicatorLength, cooldown));
        }

        private IEnumerator StartShrinkIndicator(float indicatorLength, float cooldown)
        {
            Vector2 targetScale = _originalScale;
            targetScale.x *= indicatorLength;
            Vector2 scaleVelocityVector = Vector2.zero;
            float elapsedTime = 0f;
            float smoothTime = _indicatorShrinkRate / 2;
            while (elapsedTime < cooldown)
            {
                _targetIndicator.localScale = Vector2.SmoothDamp(
                    _targetIndicator.localScale,
                    targetScale,
                    ref scaleVelocityVector,
                    smoothTime // 감속 시간
                );
                elapsedTime += Time.deltaTime; // 경과 시간 증가
                yield return null; // 다음 프레임까지 대기
            }
            _targetIndicator.localScale = targetScale;
        }
    }
}
