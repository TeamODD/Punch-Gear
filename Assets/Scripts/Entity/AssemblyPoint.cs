using System.Collections.Generic;
using System.Runtime.CompilerServices;

using PunchGear.Entity;

using UnityEngine;

namespace PunchGear
{
    public class AssemblyPoint : MonoBehaviour
    {
        [field: SerializeField]
        public EntityPosition Position { get; private set; }

        private readonly HashSet<IProjectile> _targets = new HashSet<IProjectile>();

        public bool EntersProjectile
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _targets.Count != 0;
        }

        public IEnumerable<IProjectile> ProjectileTargets
        {
            get => _targets;
        }

        private void Awake()
        {
            EntityPositionHandler.Instance.SetPosition(this, Position);
        }

        private void Start()
        {
            IProjectileLauncher launcher = ProjectileLauncher.Instance;
            launcher.OnProjectileCreated.AddListener(
                projectile =>
                {
                    projectile.OnCollisionEnter += HandleProjectileCollision;
                    projectile.OnCollisionExit += HandleProjectileCollisionExit;
                });
            launcher.OnProjectileDestroyed.AddListener(
                projectile =>
                {
                    projectile.OnCollisionEnter -= HandleProjectileCollision;
                    projectile.OnCollisionExit -= HandleProjectileCollisionExit;
                });
        }

        private void OnDisable()
        {
            _targets.Clear();
        }

        private void HandleProjectileCollision(GameObject origin, Collider2D collision)
        {
            if (collision.gameObject != gameObject)
            {
                return;
            }
            Projectile projectileImpl = origin.GetComponent<Projectile>();
            if (projectileImpl.Position != Position)
            {
                return;
            }
            projectileImpl.CanManipulate(true);
            _targets.Add(projectileImpl);
        }

        private void HandleProjectileCollisionExit(GameObject origin, Collider2D collision)
        {
            if (collision.gameObject != gameObject)
            {
                return;
            }
            Projectile projectileImpl = origin.GetComponent<Projectile>();
            if (projectileImpl.Position != Position)
            {
                return;
            }
            projectileImpl.CanManipulate(false);
            _targets.Remove(projectileImpl);
        }
    }
}
