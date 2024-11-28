using System;

using PunchGear.Attributes;

using UnityEngine;
using UnityEngine.Serialization;

namespace PunchGear
{
    public class RollableBackground : MonoBehaviour
    {
        [FormerlySerializedAs("_background")]
        [SerializeField]
        private GameObject background;

        [FormerlySerializedAs("_nextBackground")]
        [ReadOnlyField]
        [SerializeField]
        private GameObject nextBackground;

        [FormerlySerializedAs("_offset")]
        [SerializeField]
        private float offset;

        [FormerlySerializedAs("_rollSpeed")]
        [SerializeField]
        private float rollSpeed;

        private Rigidbody2D _rigidbody;
        private Rigidbody2D _nextRigidbody;

        private void Awake()
        {
            if (background == null)
            {
                throw new NullReferenceException("Sprite renderer is not attached");
            }

            nextBackground = Instantiate(background, transform);

            _rigidbody = background.GetComponent<Rigidbody2D>();
            _nextRigidbody = nextBackground.GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            Vector2 position = _nextRigidbody.position;
            position.x = offset;
            _nextRigidbody.position = position;
            _rigidbody.AddForceX(-rollSpeed);
            _nextRigidbody.AddForceX(-rollSpeed);
        }

        private void FixedUpdate()
        {
            float threshold = -offset;
            if (_rigidbody.position.x < threshold)
            {
                Vector2 currentPosition = _rigidbody.position;
                currentPosition.x += 2 * offset;
                _rigidbody.position = currentPosition;
            }
            if (_nextRigidbody.position.x < threshold)
            {
                Vector2 currentPosition = _nextRigidbody.position;
                currentPosition.x += 2 * offset;
                _nextRigidbody.position = currentPosition;
            }
        }
    }
}
