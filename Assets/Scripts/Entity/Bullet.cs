using System;

using UnityEngine;

namespace PunchGear.Entity
{
    [Obsolete]
    public class Bullet : MonoBehaviour
    {
        public float power = 1000f;
        private Camera _camera;
        private Projectile _projectile;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _projectile = GetComponent<Projectile>();
            _camera = Camera.main;
        }

        private void FixedUpdate()
        {
            Vector2 viewPosition = _camera.WorldToViewportPoint(_rigidbody.position);
            if (viewPosition.x < -1 || viewPosition.y < -1)
            {
                _projectile.ProjectilePool.Release(_projectile);
            }
        }

        private void OnEnable()
        {
            _rigidbody.AddForce(Vector2.left * power);
        }
    }
}
