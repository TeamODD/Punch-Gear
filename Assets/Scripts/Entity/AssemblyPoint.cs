using System.Collections.Generic;
using System.Runtime.CompilerServices;

using PunchGear.Entity;

using UnityEngine;

namespace PunchGear
{
    public class AssemblyPoint : MonoBehaviour, IColliderHolder
    {
        [field: SerializeField]
        public EntityPosition Position { get; private set; }

        public bool EntersProjectile
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return _targets.Count != 0;
            }
        }

        public event CollisionEnterDelegate OnCollisionEnter;

        public event CollisionExitDelegate OnCollisionExit;

        private readonly HashSet<IProjectile> _targets = new HashSet<IProjectile>();

        public IEnumerable<IProjectile> ProjectileTargets
        {
            get
            {
                return _targets;
            }
        }

        private void Awake()
        {
            EntityPositionHandler.Instance.SetPosition(this, Position);
        }

        private void OnDisable()
        {
            _targets.Clear();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            GameObject target = collider.gameObject;
            if (target.CompareTag("Projectile"))
            {
                IProjectile projectile = target.GetComponent<IProjectile>();
                _targets.Add(projectile);
            }
            OnCollisionEnter?.Invoke(collider);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            GameObject target = collider.gameObject;
            if (target.CompareTag("Projectile"))
            {
                IProjectile projectile = target.GetComponent<IProjectile>();
                _targets.Remove(projectile);
            }
            OnCollisionExit?.Invoke(collider);
        }
    }
}
