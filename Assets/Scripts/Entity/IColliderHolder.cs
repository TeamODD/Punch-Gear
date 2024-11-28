using UnityEngine;

namespace PunchGear.Entity
{
    public delegate void CollisionEnterDelegate(Collider2D collider);

    public delegate void CollisionExitDelegate(Collider2D collider);

    public interface IColliderHolder : IEntity
    {
        public event CollisionEnterDelegate OnCollisionEnter;

        public event CollisionExitDelegate OnCollisionExit;
    }
}
