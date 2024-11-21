using UnityEngine;

namespace PunchGear.Entity
{
    public delegate void CollisionEnterDelegate(Collider collider);

    public delegate void CollisionExitDelegate(Collider collider);

    public interface IColliderHolder : IEntity
    {
        public event CollisionEnterDelegate OnCollisionEnter;

        public event CollisionExitDelegate OnCollisionExit;
    }
}
