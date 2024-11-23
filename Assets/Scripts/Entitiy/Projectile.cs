using System.Collections;
using System.Runtime.CompilerServices;
using PunchGear.Enemy;
using UnityEngine;

namespace PunchGear.Entity
{
    public class Projectile : MonoBehaviour, IProjectile
    {
        private Rigidbody2D _rigidbody;
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

        public void Assemble()
        {
            if (!_canPlayerManipulate || !_disassembled || _isAssembleFrozen)
            {
                return;
            }
            _disassembled = false;
            _finalized = true;
            _animator.SetTrigger("Assemble");
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody.gravityScale = 0;
            // _renderer.sprite = _spriteProfile.AssembleImage;
            Debug.Log("Successfully assembled");
            ChaseEnemy();
        }

        public void Disassemble()
        {
            if (!_canPlayerManipulate || _disassembled || _finalized)
            {
                return;
            }
            _disassembled = true;
            _animator.SetTrigger("Disassemble");
            // _renderer.sprite = _spriteProfile.DisassembleImage;
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody.AddForceY(10f, ForceMode2D.Impulse);
            _rigidbody.gravityScale = 2f;
            _isAssembleFrozen = true;
            FreezeAssemble();
            Debug.Log("Successfully disassembled");
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
            float entireAngle = 360;
            float angleDelta = entireAngle * SpinRate;
            while (_rigidbody.rotation < entireAngle)
            {
                float angle = _rigidbody.rotation + angleDelta * Time.deltaTime;
                _rigidbody.SetRotation(angle);
                yield return null;
            }
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.SetRotation(0);
            yield return null;
            while (true)
            {
                transform.position = Vector2.SmoothDamp(
                    transform.position,
                    EnemyOrigin.transform.position,
                    ref velocityVector,
                    smoothTime // 감속 시간
                );
                yield return null;
            }
        }
    }
}
