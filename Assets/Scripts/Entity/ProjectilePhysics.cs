using UnityEngine;

namespace PunchGear
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProjectilePhysics : MonoBehaviour
    {
        [SerializeField]
        private float velocity;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            if (_rigidbody == null)
            {
                throw new UnassignedReferenceException("ProjectilePhysics requires a rigidbody");
            }
        }

        private void OnEnable()
        {
            float deltaTime = Time.fixedDeltaTime;
            _rigidbody.AddForceX(-velocity / deltaTime);
        }
    }
}
