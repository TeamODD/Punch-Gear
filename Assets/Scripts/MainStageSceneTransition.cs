using PunchGear.Entity;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace PunchGear
{
    public class MainStageSceneTransition : MonoBehaviour
    {
        private Nobility _nobility;
        private Player _player;

        private void Awake()
        {
            _player = FindFirstObjectByType<Player>();
            _nobility = FindFirstObjectByType<Nobility>();
        }

        private void Start()
        {
            _player.OnHealthChange += HandlePlayerHealth;
            _nobility.OnHealthChange += HandleEnemyHealth;
        }

        private void OnDisable()
        {
            _player.OnHealthChange -= HandlePlayerHealth;
            _nobility.OnHealthChange -= HandleEnemyHealth;
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
                SceneManager.LoadScene("GameClear");
            }
        }
    }
}
