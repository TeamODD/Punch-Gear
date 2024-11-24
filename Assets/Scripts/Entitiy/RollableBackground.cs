using System;
using UnityEngine;

namespace PunchGear
{
    public class RollableBackground : MonoBehaviour
    {
        [SerializeField]
        private GameObject _background;

        [SerializeField]
        private float _offset;
        [SerializeField]
        private float _rollSpeed;

        private Rigidbody2D _rigidbody;

        private GameObject _nextBackground;

        private Rigidbody2D _nextRigidbody;

        private void Awake()
        {
            if (_background == null)
            {
                throw new NullReferenceException("Sprite renderer is not attached");
            }

            _nextBackground = Instantiate(_background, transform);

            _rigidbody = _background.GetComponent<Rigidbody2D>();
            _nextRigidbody = _nextBackground.GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            Vector2 position = _nextRigidbody.position;
            position.x = _offset;
            _nextRigidbody.position = position;
            _rigidbody.AddForceX(-_rollSpeed);
            _nextRigidbody.AddForceX(-_rollSpeed);
        }

        private void FixedUpdate()
        {
            float threshold = -_offset;
            if (_rigidbody.position.x < threshold)
            {
                Vector2 currentPosition = _rigidbody.position;
                currentPosition.x += 2 * _offset;
                _rigidbody.position = currentPosition;
            }
            if (_nextRigidbody.position.x < threshold)
            {
                Vector2 currentPosition = _nextRigidbody.position;
                currentPosition.x += 2 * _offset;
                _nextRigidbody.position = currentPosition;
            }
        }
    }
}
