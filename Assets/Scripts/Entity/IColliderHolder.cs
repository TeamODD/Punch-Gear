using UnityEngine;

namespace PunchGear.Entity
{
    public delegate void CollisionEnterDelegate(GameObject gameObject, Collider2D collider);

    public delegate void CollisionExitDelegate(GameObject gameObject, Collider2D collider);

    public interface IColliderHolder : IEntity
    {
        public event CollisionEnterDelegate OnCollisionEnter;

        public event CollisionExitDelegate OnCollisionExit;
    }
}
