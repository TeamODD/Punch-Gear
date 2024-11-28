using System;
using System.Collections;

using PunchGear.Enemy;

using UnityEngine;
using UnityEngine.Serialization;

namespace PunchGear.Entity.UI
{
    public class NobilityHealthIndicator : MonoBehaviour
    {
        private EnemyObject _enemyObject;

        [FormerlySerializedAs("_targetIndicator")]
        [SerializeField]
        private Transform targetIndicator;

        [FormerlySerializedAs("_indicatorShrinkRate")]
        [Range(0.01f, 1f)]
        [SerializeField]
        private float indicatorShrinkRate;

        private Vector3 _originalScale;
        private int _originalNobilityHealth;

        private void Awake()
        {
            _enemyObject = FindFirstObjectByType<EnemyObject>();
            if (_enemyObject == null)
            {
                throw new NullReferenceException("Cannot find enemy object");
            }
            if (targetIndicator == null)
            {
                throw new NullReferenceException("Indicator transform is not attached");
            }
            _originalScale = targetIndicator.localScale;
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
            StartIndicateShrink(currentHealth, indicatorShrinkRate);
        }

        public void StartIndicateShrink(int health, float cooldown)
        {
            float indicatorLength = health / (float) _originalNobilityHealth;
            StartCoroutine(StartShrinkIndicator(indicatorLength, cooldown));
        }

        private IEnumerator StartShrinkIndicator(float indicatorLength, float cooldown)
        {
            Vector2 targetScale = _originalScale;
            targetScale.x *= indicatorLength;
            Vector2 scaleVelocityVector = Vector2.zero;
            float elapsedTime = 0f;
            float smoothTime = indicatorShrinkRate / 2;
            while (elapsedTime < cooldown)
            {
                targetIndicator.localScale = Vector2.SmoothDamp(
                    targetIndicator.localScale,
                    targetScale,
                    ref scaleVelocityVector,
                    smoothTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            targetIndicator.localScale = targetScale;
        }
    }
}
