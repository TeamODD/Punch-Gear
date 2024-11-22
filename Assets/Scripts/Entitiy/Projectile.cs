using System;
using System.Collections;
using UnityEngine;

namespace PunchGear.Entity
{
    public class Projectile : MonoBehaviour, IProjectile
    {
        [SerializeField]
        private ProjectileSpriteProfile _spriteProfile;

        private Rigidbody2D _rigidbody;
        private SpriteRenderer _renderer;
        private IMouseInputAction _action;

        private bool _canPlayerManipulate;
        private bool _disassembled;

        private void Awake()
        {
            if (_spriteProfile == null)
            {
                throw new NullReferenceException("Sprite profile is not attached");
            }
            _rigidbody = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _action = new MouseInputAction(this);
            _canPlayerManipulate = false;
            _disassembled = false;

            GloballyPlayerInputHandler globallyPlayerInputHandler = GloballyPlayerInputHandler.Instance;
            globallyPlayerInputHandler.AddAction(_action);
        }

        private void Start()
        {
            _renderer.sprite = _spriteProfile.DefaultImage;
            _canPlayerManipulate = false;
            _disassembled = false;
        }

        private void OnDisable()
        {
            GloballyPlayerInputHandler globallyPlayerInputHandler = GloballyPlayerInputHandler.Instance;
            globallyPlayerInputHandler.RemoveAction(_action);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            GameObject target = collider.gameObject;
            if (target.CompareTag("GameController"))
            {
                _canPlayerManipulate = true;
            }
            Debug.LogFormat("Collider detected: tag {0}, name {1}", target.tag, target.name);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            GameObject target = collider.gameObject;
            if (target.CompareTag("GameController"))
            {
                _canPlayerManipulate = false;
                // StartCoroutine(StartBlinkAnimationCoroutine());
            }
        }

        public void Assemble()
        {
            if (!_canPlayerManipulate || !_disassembled)
            {
                return;
            }
            _disassembled = false;
            _renderer.sprite = _spriteProfile.AssembleImage;
            // TODO: attacks the enemy
        }

        public void Disassemble()
        {
            if (!_canPlayerManipulate || _disassembled)
            {
                return;
            }
            _disassembled = true;
            _renderer.sprite = _spriteProfile.DisassembleImage;
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody.AddForceY(10f, ForceMode2D.Impulse);
            _rigidbody.gravityScale = 2f;
            // TODO: awaits player's assembly or explode
        }

        private IEnumerator StartBlinkAnimationCoroutine()
        {
            Material material = _renderer.material;
            for (int i = 0; i < 10; i++)
            {
                Color color = material.color;
                color.a = 0;
                material.color = color;
                yield return new WaitForSecondsRealtime(0.1f);
                color = material.color;
                color.a = 1f;
                material.color = color;
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        private class MouseInputAction : IMouseInputAction
        {
            private readonly IProjectile _projectile;

            public MouseInputAction(IProjectile projectile)
            {
                _projectile = projectile;
            }

            public void OnMouseDown(MouseInputs inputs)
            {
                if (inputs == MouseInputs.Left)
                {
                    _projectile.Disassemble();
                }
                else if (inputs == MouseInputs.Right)
                {
                    _projectile.Assemble();
                }
            }
        }
    }
}
