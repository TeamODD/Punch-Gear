using UnityEngine;

namespace PunchGear.Entity
{
    public class Bullet : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private Projectile _projectile;
        private Camera _camera;

        public float power = 1000f;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _projectile = GetComponent<Projectile>();
            _camera = Camera.main;
        }

        private void Start()
        {
            _rigidbody.AddForce(Vector2.left * power);
        }

        private void FixedUpdate()
        {
            Vector2 viewPosition = _camera.WorldToViewportPoint(_rigidbody.position);
            if (viewPosition.x < -1 || viewPosition.y < -1)
            {
                _projectile.ProjectilePool.Release(_projectile);
            }
        }
    }
}
