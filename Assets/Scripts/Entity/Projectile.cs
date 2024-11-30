using System.Collections;

using PunchGear.Attributes;
using PunchGear.Enemy;

using UnityEngine;
using UnityEngine.Pool;

namespace PunchGear.Entity
{
    [RequireComponent(typeof(ProjectilePhysics))]
    public class Projectile : MonoBehaviour, IProjectile, IPoolingObject
    {
        private static readonly int DisassembleAnimation = Animator.StringToHash("Disassemble");
        private static readonly int AssembleAnimation = Animator.StringToHash("Assemble");
        private static readonly int IdleAnimation = Animator.StringToHash("Idle");

        [field: SerializeField]
        public float AssembleFreezeCooldown { get; private set; }

        [field: ReadOnlyField]
        [field: SerializeField]
        public GameObject EnemyOrigin { get; set; }

        private Animator _animator;
        private bool _canPlayerManipulate;
        private Coroutine _chaseEnemyAnimationCoroutine;
        private bool _isAssembleFrozen;
        private Rigidbody2D _rigidbody;

        public IObjectPool<IProjectile> ProjectilePool { get; set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        public void OnEnable()
        {
            _canPlayerManipulate = false;
            _isAssembleFrozen = false;
            State = ProjectileState.Launched;
            Disassembled = false;
            Assembled = false;

            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody.gravityScale = 0;
            _animator.SetTrigger(IdleAnimation);
        }

        public void OnDisable()
        {
            if (_chaseEnemyAnimationCoroutine == null)
            {
                return;
            }
            StopCoroutine(_chaseEnemyAnimationCoroutine);
            _chaseEnemyAnimationCoroutine = null;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            OnCollisionEnter?.Invoke(collider);
            GameObject target = collider.gameObject;
            if (target.CompareTag("GameController"))
            {
                AssemblyPoint assemblyPoint = target.GetComponent<AssemblyPoint>();
                if (assemblyPoint.Position != Position)
                {
                    return;
                }
                _canPlayerManipulate = true;
            }
            if (target.CompareTag("Nobility"))
            {
                if (Disassembled || _chaseEnemyAnimationCoroutine == null)
                {
                    return;
                }
                StopCoroutine(_chaseEnemyAnimationCoroutine);
                ProjectilePool.Release(this);
                EnemyObject enemyObject = EnemyOrigin.GetComponent<EnemyObject>();
                enemyObject.Health -= 1;
            }
            if (target.CompareTag("Player"))
            {
                if (Disassembled || Assembled)
                {
                    return;
                }
                Player player = target.GetComponentInParent<Player>();
                player.Health -= 1;
                ProjectilePool.Release(this);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            OnCollisionExit?.Invoke(collider);
            GameObject target = collider.gameObject;
            if (target.CompareTag("GameController"))
            {
                _canPlayerManipulate = false;
            }
        }

        [field: ReadOnlyField]
        [field: SerializeField]
        public EntityPosition Position { get; set; }

        [field: ReadOnlyField]
        [field: SerializeField]
        public ProjectileState State { get; private set; }

        [field: ReadOnlyField]
        [field: SerializeField]
        public bool Disassembled { get; private set; }

        [field: ReadOnlyField]
        [field: SerializeField]
        public bool Assembled { get; private set; }

        public event CollisionEnterDelegate OnCollisionEnter;

        public event CollisionExitDelegate OnCollisionExit;

        public void Assemble()
        {
            if (!_canPlayerManipulate || !Disassembled || _isAssembleFrozen)
            {
                return;
            }
            Disassembled = false;
            Assembled = true;
            State = ProjectileState.Assembled;
            _animator.SetTrigger(AssembleAnimation);
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody.gravityScale = 0;
            ChaseEnemy();
        }

        public void Disassemble()
        {
            if (!_canPlayerManipulate || Disassembled || Assembled)
            {
                return;
            }
            Disassembled = true;
            State = ProjectileState.Disassembled;
            _animator.SetTrigger(DisassembleAnimation);
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody.AddForceY(10f, ForceMode2D.Impulse);
            _rigidbody.gravityScale = 2f;
            _isAssembleFrozen = true;
            FreezeAssemble();
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
                    smoothTime);
                yield return null;
            }
        }
    }
}
