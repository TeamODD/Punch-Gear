using System;
using System.Collections;
using PunchGear.Enemy;
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

        private Coroutine _chaseEnemyAnimationCoroutine;

        private bool _canPlayerManipulate;
        private bool _disassembled;

        [field: SerializeField]
        public EntityPosition Position { get; set; }

        [field: SerializeField]
        public GameObject EnemyOrigin { get; set; }

        [field: SerializeField]
        public Player Player { get; set; }

        private PlayerMoveController _playerMoveController;

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
            _playerMoveController = Player.GetComponent<PlayerMoveController>();
        }

        private void OnDisable()
        {
            ProjectileLauncher.Instance.OnProjectileDestroyed.Invoke(this);
            GloballyPlayerInputHandler globallyPlayerInputHandler = GloballyPlayerInputHandler.Instance;
            globallyPlayerInputHandler.RemoveAction(_action);
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
                if (!_disassembled && _chaseEnemyAnimationCoroutine != null)
                {
                    StopCoroutine(_chaseEnemyAnimationCoroutine);
                    Destroy(gameObject);
                    EnemyObject enemyObject = EnemyOrigin.GetComponent<EnemyObject>();
                    enemyObject.Health -= 1;
                }
            }
            if (target.CompareTag("Player"))
            {
                if (_disassembled)
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
            if (!_canPlayerManipulate || !_disassembled)
            {
                return;
            }
            _disassembled = false;
            _renderer.sprite = _spriteProfile.AssembleImage;
            Debug.Log("Successfully assembled");
            ChaseEnemy();
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
            Debug.Log("Successfully disassembled");
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
            while (true)
            {
                transform.position = Vector2.SmoothDamp(
                    transform.position,
                    EnemyOrigin.transform.position,
                    ref velocityVector,
                    smoothTime // 감속 시간
                );
                // 
                yield return null;
            }
        }

        private class MouseInputAction : IMouseInputAction
        {
            private readonly Projectile _projectile;

            public MouseInputAction(Projectile projectile)
            {
                _projectile = projectile;
            }

            public void OnMouseDown(MouseInputs inputs)
            {
                if (_projectile.Position != _projectile._playerMoveController.Position)
                {
                    return;
                }
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
