using System.Collections;

using PunchGear.Attributes;

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

        private Camera _camera;
        private bool _canPlayerManipulate;
        private Coroutine _chaseEnemyAnimationCoroutine;
        private bool _isAssembleFrozen;
        private Rigidbody2D _rigidbody;

        public IObjectPool<IProjectile> ProjectilePool { get; set; }

        private void Awake()
        {
            _camera = Camera.main;
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            Vector2 viewPosition = _camera.WorldToViewportPoint(_rigidbody.position);
            if (viewPosition.x < -1 || viewPosition.y < -1)
            {
                ProjectilePool.Release(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            OnCollisionEnter?.Invoke(gameObject, collider);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            OnCollisionExit?.Invoke(gameObject, collider);
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

        public void CanManipulate(bool canPlayerManipulate)
        {
            _canPlayerManipulate = canPlayerManipulate;
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
