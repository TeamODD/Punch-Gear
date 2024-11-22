using UnityEngine;

namespace PunchGear.Entity
{
    public class Bullet : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        public float power = 1000f;
        public float removeTime = 3f;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            _rigidbody.AddForce(Vector2.left * power);
        }

        private void FixedUpdate()
        {
            Vector2 position = _rigidbody.position;
            if (position.x < -10 || position.y < -6)
            {
                Debug.Log("Bullet is out of screen");
                Destroy(gameObject);
            }
        }
    }
}
