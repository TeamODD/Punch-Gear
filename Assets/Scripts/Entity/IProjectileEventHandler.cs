using UnityEngine.Events;

namespace PunchGear.Entity
{
    public interface IProjectileEventHandler
    {
        public UnityEvent<IProjectile> OnProjectileCreated { get; }

        public UnityEvent<IProjectile> OnProjectileDestroyed { get; }
    }
}
