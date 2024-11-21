using System;
using UnityEngine;

namespace PunchGear.Entity
{
    public class Projectile : MonoBehaviour, IProjectile
    {
        [SerializeField]
        private ProjectileSpriteProfile _spriteProfile;

        private Rigidbody2D _rigidbody;
        private SpriteRenderer _renderer;

        private bool _disassembled;

        private void Awake()
        {
            if (_spriteProfile == null)
            {
                throw new NullReferenceException("Sprite profile is not attached");
            }
            _rigidbody = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _disassembled = false;
        }

        private void Start()
        {
            // TODO: trace the track
        }

        public void Assemble()
        {
            _disassembled = false;
            _renderer.sprite = _spriteProfile.AssembleImage;
            // TODO: attacks the enemy
        }

        public void Disassemble()
        {
            _disassembled = true;
            _renderer.sprite = _spriteProfile.DisassembleImage;
            // TODO: awaits player's assembly or explode
        }
    }
}
