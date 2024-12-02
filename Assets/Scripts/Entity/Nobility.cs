using PunchGear.Attributes;
using PunchGear.Enemy;

using UnityEngine;

namespace PunchGear.Entity
{
    [RequireComponent(typeof(NobilityAnimationController))]
    [RequireComponent(typeof(EnemyPattern))]
    public class Nobility : HealthHolderBase
    {
        [ReadOnlyField]
        [SerializeField]
        private bool componentEnabled;

        [SerializeField]
        private Collider2D collider;

        private NobilityAnimationController _animationController;
        private EnemyPattern _enemyPattern;

        private void Awake()
        {
            _animationController = GetComponent<NobilityAnimationController>();
            _enemyPattern = GetComponent<EnemyPattern>();
        }

        private void Start()
        {
            ProjectileLauncher.Instance.OnProjectileCreated.AddListener(
                projectile => projectile.OnCollisionEnter += HandleHealth);
            ProjectileLauncher.Instance.OnProjectileDestroyed.AddListener(
                projectile => projectile.OnCollisionEnter -= HandleHealth);
        }

        private void OnEnable()
        {
            componentEnabled = true;
        }

        private void OnDisable()
        {
            componentEnabled = false;
        }

        private void HandleHealth(GameObject origin, Collider2D collider)
        {
            if (!componentEnabled)
            {
                return;
            }
            if (collider.gameObject != this.collider.gameObject)
            {
                return;
            }
            Projectile projectileImpl = origin.GetComponent<Projectile>();
            if (projectileImpl.State != ProjectileState.Assembled)
            {
                return;
            }
            projectileImpl.ProjectilePool.Release(projectileImpl);
            Health -= 1;
        }
    }
}
