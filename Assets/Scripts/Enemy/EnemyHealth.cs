using System;

using UnityEngine;

using System.Collections;

using PunchGear.Internal;

namespace PunchGear.Enemy
{
    [Obsolete]
    public class EnemyHealth : MonoBehaviour
    {
        public GameManager totalManager;
        private float HealthBarLength;

        private float scaleControl;
        private float positionControl;
        private Vector3 positionVelocity = Vector3.zero;
        private Vector3 scaleVelocity = Vector3.zero;
        public float duration = 0.6f;

        private void Start()
        {
            HealthBarLength = transform.localScale.x;

            scaleControl = HealthBarLength / totalManager.enemyHealthMax;
            positionControl = scaleControl * 6.5f;

            StartCoroutine(HealthHandler());
        }

        public IEnumerator HealthHandler()
        {
            Vector3 targetScale = transform.localScale - new Vector3(scaleControl, 0, 0);
            Vector3 targetPosition = transform.position - new Vector3(positionControl, 0, 0);

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    targetPosition,
                    ref positionVelocity,
                    0.2f);

                transform.localScale = Vector3.SmoothDamp(
                    transform.localScale,
                    targetScale,
                    ref scaleVelocity,
                    0.2f);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = targetScale;
            transform.localPosition = targetPosition;
        }
    }
}
