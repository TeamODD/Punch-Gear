using PunchGear.Enemy;
using PunchGear.Entity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PunchGear
{
    public class MainStageSceneTransition : MonoBehaviour
    {
        private Player _player;
        private EnemyObject _enemyObject;

        private void Awake()
        {

            _player = FindFirstObjectByType<Player>();
            _enemyObject = FindFirstObjectByType<EnemyObject>();
        }

        private void Start()
        {
            _player.OnHealthChange += HandlePlayerHealth;
            _enemyObject.OnHealthChange += HandleEnemyHealth;
        }

        private void OnDisable()
        {
            _player.OnHealthChange -= HandlePlayerHealth;
            _enemyObject.OnHealthChange -= HandleEnemyHealth;
        }

        private void HandlePlayerHealth(int _, int health)
        {
            if (health == 0)
            {
                SceneManager.LoadScene("GameOver");
            }
        }

        private void HandleEnemyHealth(int _, int health)
        {
            if (health == 0)
            {
                SceneManager.LoadScene("GaemClear");
            }
        }
    }
}
