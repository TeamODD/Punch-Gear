using System;
using System.Collections;
using System.Runtime.CompilerServices;
using PunchGear.Enemy;
using UnityEngine;

namespace PunchGear.Entity
{
    public class Projectile : MonoBehaviour, IProjectile
    {
        private static AudioClip DisassembleAudioClip;
        private static AudioClip AssembleAudioClip;

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

        [field: SerializeField]
        public float SpinRate
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void LoadAudioClips()
        {
            DisassembleAudioClip = Resources.Load<AudioClip>("Sound/분해");
            AssembleAudioClip = Resources.Load<AudioClip>("Sound/조립");
            if (DisassembleAudioClip == null)
            {
                throw new NullReferenceException("Cannot find Audio clip in the path");
            }
            if (AssembleAudioClip == null)
            {
                throw new NullReferenceException("Cannot find Audio clip in the path");
            }
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

        public void Assemble()
        {
            if (!_canPlayerManipulate || !_disassembled || _isAssembleFrozen)
            {
                return;
            }
            _disassembled = false;
            _finalized = true;
            AudioManager.Instance.Play(AssembleAudioClip);
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
            AudioManager.Instance.Play(DisassembleAudioClip);
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
            // float entireAngle = 360;
            // float angleDelta = entireAngle * SpinRate;
            // while (_spriteRigidbody.rotation < entireAngle)
            // {
            //     float angle = _spriteRigidbody.rotation + angleDelta * Time.deltaTime;
            //     _spriteRigidbody.SetRotation(angle);
            //     yield return null;
            // }
            // _spriteRigidbody.linearVelocity = Vector2.zero;
            // _spriteRigidbody.SetRotation(0);
            // yield return null;
            yield return new WaitForSecondsRealtime(0.2f);
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
