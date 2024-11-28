using System;
using System.Collections;

using UnityEngine;

namespace PunchGear.Internal
{
    [Obsolete]
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField]
        private GameObject _gameManagerObject;

        [SerializeField]
        private GameObject[] _childObject = new GameObject[4];

        [SerializeField]
        private Sprite _broke;

        private GameManager _gameManager;

        public float duration = 0.2f;

        private void Start()
        {
            _gameManager = _gameManagerObject.GetComponent<GameManager>();

            for (int i = 0; i < 4; i++)
            {
                _childObject[i] = transform.GetChild(i).gameObject;
            }

            StartCoroutine(HealthControl());
            StartCoroutine(HealthControl());
        }

        private IEnumerator HealthControl()
        {
            GameObject healthObject = _childObject[_gameManager.playerHealthMax - _gameManager.playerHealth];
            _gameManager.playerHealth--;

            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(duration);
                healthObject.SetActive(false);
                yield return new WaitForSeconds(duration);
                healthObject.SetActive(true);
            }

            Destroy(healthObject);
            switch (_gameManager.playerHealth)
            {
                case 0: break; // 대충 게임 오버
                case 1: HealthBarBreak(); break;
            }
        }

        private void HealthBarBreak()
        {
            SpriteRenderer spriteRenderer = _childObject[3].GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = _broke;
        }
    }
}
