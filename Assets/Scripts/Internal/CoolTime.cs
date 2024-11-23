using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PunchGear
{
    public class CoolTime : MonoBehaviour
    {
        public GameObject gameManagerObject;
        private Internal.GameManager _gameManager;

        private float _scaleControl; // 이건 지금부터 스케일 조정기여
        private float _positionControl; // 너는 지금부터 위치 조정기여
        private Vector3 _positionVelocity = Vector3.zero; // 위치에 대한 속도 추적 변수
        private Vector3 _scaleVelocity = Vector3.zero;    // 스케일에 대한 속도 추적 변수
        public float duration;

        private Vector3 _positionOrigin;
        private Vector3 _scaleOrigin;

        private void Start()
        {
            _gameManager = gameManagerObject.GetComponent<Internal.GameManager>();

            _positionOrigin = this.transform.position;
            _scaleOrigin = this.transform.localScale;

            _scaleControl = _scaleOrigin.x;
            _positionControl = _scaleControl * 1.77f;

            StartCoroutine(CoolTimeHandler());
        }

        public IEnumerator CoolTimeHandler()
        {
            Vector3 targetScale = new Vector3(0f, 1f, 1f);
            Vector3 targetPosition = this.transform.localPosition - new Vector3(_positionControl, 0f, 0f);

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                // SmoothDamp를 통해 부드럽게 이동
                transform.localPosition = Vector3.SmoothDamp(
                    this.transform.localPosition,
                    targetPosition,
                    ref _positionVelocity,
                    0.2f // 감속 시간
                );

                transform.localScale = Vector3.SmoothDamp(
                    this.transform.localScale,
                    targetScale,
                    ref _scaleVelocity,
                    0.2f // 감속 시간
                );

                elapsedTime += Time.deltaTime; // 경과 시간 증가
                yield return null; // 다음 프레임까지 대기
            }

            transform.localScale = targetScale;
            transform.localPosition = targetPosition;
        }

        public void CoolDownStart()
        {
            // 이 함수 외부에서 쿨타임 0인지 확인해야함
            // 0인지 확인하는 방법은 transform.localScale.x가 0인지 확인하면 됨
            transform.localPosition = _positionOrigin;
            transform.localScale = _scaleOrigin;
        }
    }
}
