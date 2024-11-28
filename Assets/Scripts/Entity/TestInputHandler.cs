using System;
using System.Collections.Generic;

using UnityEngine;

namespace PunchGear.Entity
{
    [Obsolete]
    public class TestInputHandler : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _renderer;

        private Rigidbody2D _rigidBody;

        private void Awake()
        {
            if (_renderer == null)
            {
                throw new NullReferenceException("SpriteRenderer is not attached");
            }
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            GloballyPlayerInputHandler.Instance.AddAction(new SpriteAction(_renderer));
            GloballyPlayerInputHandler.Instance.AddAction(new RigidbodyAction(_rigidBody));
        }

        private class SpriteAction : IMouseInputAction
        {
            private readonly SpriteRenderer _renderer;

            public SpriteAction(SpriteRenderer renderer)
            {
                _renderer = renderer;
            }

            public void OnMouseDown(MouseInputs inputs)
            {
                if (inputs.HasFlag(MouseInputs.Left))
                {
                    _renderer.flipX = false;
                }
                else if (inputs.HasFlag(MouseInputs.Right))
                {
                    _renderer.flipX = true;
                }
            }
        }

        private class RigidbodyAction : IKeyboardInputAction
        {
            private readonly Rigidbody2D _rigidbody;

            public RigidbodyAction(Rigidbody2D rigidbody)
            {
                _rigidbody = rigidbody;
            }

            public void OnKeyDown(IList<KeyCode> keyCodes)
            {
                if (keyCodes.Contains(KeyCode.W))
                {
                    _rigidbody.position += Vector2.up * (4 * Time.fixedDeltaTime);
                }
                else if (keyCodes.Contains(KeyCode.S))
                {
                    _rigidbody.position += Vector2.down * (4 * Time.fixedDeltaTime);
                }
            }
        }
    }
}
