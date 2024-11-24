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

        public bool EntersProjectile
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _targets.Count != 0;
        }

        private readonly HashSet<Projectile> _targets = new HashSet<Projectile>();

        public IEnumerable<Projectile> ProjectileTargets => _targets;

        private void Start()
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
                Projectile projectile = target.GetComponent<Projectile>();
                _targets.Add(projectile);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            GameObject target = collider.gameObject;
            if (target.CompareTag("Projectile"))
            {
                Projectile projectile = target.GetComponent<Projectile>();
                _targets.Remove(projectile);
            }
        }
    }
}
