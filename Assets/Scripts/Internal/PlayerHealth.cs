using PunchGear.Internal;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace PunchGear.Internal
{
    public class PlayerHealth : MonoBehaviour
    {
        public GameManager gameManager;
        public GameObject[] childObject = new GameObject[3];
        public float duration = 0.2f;
        private void Start()
        {
            for (int i = 0; i < 3; i++)
            {
                childObject[i] = transform.GetChild(i).gameObject;
            }

            StartCoroutine(HealthControl());
            StartCoroutine(HealthControl());
        }

        private IEnumerator HealthControl()
        {
            GameObject healthObject = childObject[gameManager.playerHealthMax - gameManager.playerHealth];
            gameManager.playerHealth--;

            for (int i = 0; i < 3; i++) 
            {
                yield return new WaitForSeconds(duration);
                healthObject.SetActive(false);
                yield return new WaitForSeconds(duration);
                healthObject.SetActive(true);
            }

            Destroy(healthObject);

            if (gameManager.playerHealth == 0)
            {
                // 대충 게임 오버

            }
        }
    }
}
