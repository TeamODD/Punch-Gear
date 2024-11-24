using System;
using System.Collections;
using System.Runtime.CompilerServices;
using PunchGear.Enemy;
using UnityEngine;

namespace PunchGear.Entity
{
    public class Projectile : MonoBehaviour, IProjectile
    {

        private Rigidbody2D _rigidbody;
        [SerializeField]
        private Rigidbody2D _spriteRigidbody;
        private Animator _animator;

        private Coroutine _chaseEnemyAnimationCoroutine;

        private bool _canPlayerManipulate;
        private bool _disassembled;
        private bool _isAssembleFrozen;
        private bool _finalized;

        [field: SerializeField]
        public EntityPosition Position { get; set; }

        [field: SerializeField]
        public GameObject EnemyOrigin { get; set; }

        [field: SerializeField]
        public Player Player { get; set; }

        [field: SerializeField]
        public float AssembleFreezeCooldown { get; private set; }

        public bool Disassembled => _disassembled;

        public bool Assembled => _finalized;

        [field: SerializeField]
        public float SpinRate
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set;
        }

        private void Awake()
        {
            if (_spriteRigidbody == null)
            {
                throw new NullReferenceException("Rigidbody of sprite is not attached");
            }
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _canPlayerManipulate = false;
            _disassembled = false;
            _isAssembleFrozen = false;
            _finalized = false;
        }

        private void Start()
        {
            _canPlayerManipulate = false;
            _disassembled = false;
            _isAssembleFrozen = false;
            _finalized = false;
        }

        private void OnDisable()
        {
            ProjectileLauncher.Instance.OnProjectileDestroyed.Invoke(this);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            GameObject target = collider.gameObject;
            if (target.CompareTag("GameController"))
            {
                AssemblyPoint assemblyPoint = target.GetComponent<AssemblyPoint>();
                if (assemblyPoint.Position != Position)
                {
                    return;
                }
                Debug.Log("Projectile entered the assembly point: " + nameof(Position));
                _canPlayerManipulate = true;
            }
            if (target.CompareTag("Nobility"))
            {
                if (_disassembled || _chaseEnemyAnimationCoroutine == null)
                {
                    return;
                }
                StopCoroutine(_chaseEnemyAnimationCoroutine);
                Destroy(gameObject);
                EnemyObject enemyObject = EnemyOrigin.GetComponent<EnemyObject>();
                enemyObject.Health -= 1;
            }
            if (target.CompareTag("Player"))
            {
                if (_disassembled || _finalized)
                {
                    return;
                }
                Player player = target.GetComponentInParent<Player>();
                player.Health -= 1;
                Destroy(gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            GameObject target = collider.gameObject;
            if (target.CompareTag("GameController"))
            {
                _canPlayerManipulate = false;
            }
        }

        public bool Assemble()
        {
            if (!_canPlayerManipulate || !_disassembled || _isAssembleFrozen)
            {
                return false;
            }
            _disassembled = false;
            _finalized = true;
            _animator.SetTrigger("Assemble");
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody.gravityScale = 0;
            ChaseEnemy();
            return true;
        }

        public bool Disassemble()
        {
            if (!_canPlayerManipulate || _disassembled || _finalized)
            {
                return false;
            }
            _disassembled = true;
            _animator.SetTrigger("Disassemble");
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody.AddForceY(10f, ForceMode2D.Impulse);
            _rigidbody.gravityScale = 2f;
            _isAssembleFrozen = true;
            FreezeAssemble();
            return true;
        }

        private void FreezeAssemble()
        {
            StartCoroutine(FreezeAssembleCoroutine());
        }

        private IEnumerator FreezeAssembleCoroutine()
        {
            yield return new WaitForSecondsRealtime(AssembleFreezeCooldown);
            _isAssembleFrozen = false;
        }

        private void ChaseEnemy()
        {
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.gravityScale = 0;
            _chaseEnemyAnimationCoroutine = StartCoroutine(ChaseEnemyAnimation());
        }

        private IEnumerator ChaseEnemyAnimation()
        {
            float smoothTime = 0.2f;
            Vector2 velocityVector = Vector2.zero;
            yield return new WaitForSecondsRealtime(0.2f);
            while (true)
            {
                transform.position = Vector2.SmoothDamp(
                    transform.position,
                    EnemyOrigin.transform.position,
                    ref velocityVector,
                    smoothTime
                );
                yield return null;
            }
        }
    }
}
