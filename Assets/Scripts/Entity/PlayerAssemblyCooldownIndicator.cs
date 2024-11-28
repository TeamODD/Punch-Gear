using System;
using System.Collections;

using UnityEngine;

namespace PunchGear.Entity
{
    public class PlayerAssemblyCooldownIndicator : MonoBehaviour
    {
        [SerializeField]
        private Transform _targetIndicator;

        private Vector3 _originalScale;

        private void Awake()
        {
            if (_targetIndicator == null)
            {
                throw new NullReferenceException("Indicator transform is not attached");
            }
            _originalScale = _targetIndicator.localScale;
        }

        public Coroutine StartIndicateCooldown(float cooldown)
        {
            return StartCoroutine(StartShrinkIndicator(cooldown));
        }

        private IEnumerator StartShrinkIndicator(float cooldown)
        {
            Vector2 targetScale = _originalScale;
            targetScale.x = 0f;
            Vector2 scaleVelocityVector = Vector2.zero;
            float elapsedTime = 0f;
            while (elapsedTime < cooldown)
            {
                _targetIndicator.localScale = Vector2.SmoothDamp(
                    _targetIndicator.localScale,
                    targetScale,
                    ref scaleVelocityVector,
                    0.2f);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _targetIndicator.localScale = _originalScale;
        }
    }
}
