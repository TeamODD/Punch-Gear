using UnityEngine;

namespace PunchGear.Entity
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Weapon : MonoBehaviour, IColliderHolder
    {
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;

        public event CollisionEnterDelegate OnCollisionEnter;
        public event CollisionExitDelegate OnCollisionExit;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            OnCollisionEnter?.Invoke(collider);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            OnCollisionExit?.Invoke(collider);
        }
    }
}
