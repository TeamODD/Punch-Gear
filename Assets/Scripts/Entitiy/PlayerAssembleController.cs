using UnityEngine;

namespace PunchGear.Entity
{
    public class PlayerAssembleController : MonoBehaviour
    {
        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void Start()
        {
            ProjectileLauncher launcher = ProjectileLauncher.Instance;
            launcher.OnProjectileCreated.AddListener(projectile =>
            {
                // 이제 여기다가 하면 되는 부분
                Debug.Log("Projectile launched");
            });
            launcher.OnProjectileDestroyed.AddListener(projectile =>
            {
                // 이제 여기다가 하면 되는 부분
                Debug.Log("Projectile destroyed");
            });
        }

        private class MouseInputAction : IMouseInputAction
        {
            private readonly Projectile _projectile;

            public MouseInputAction(Projectile projectile)
            {
                _projectile = projectile;
            }

            public void OnMouseDown(MouseInputs inputs)
            {
            }
        }
    }
}
