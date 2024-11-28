using System;
using System.Collections;
using System.Collections.Generic;

using PunchGear.Entity;

using UnityEngine;

namespace PunchGear.Enemy
{
    public class EnemyPattern : MonoBehaviour, IPlaceableEntity
    {
        public float slow = 1.2f;
        public float normal = 1.0f;
        public float fast = 0.8f;
        public float duration = 1f;
        public float term = 1.5f;
        public float smoothTime = 0.2f;

        private readonly List<IAttackPattern> _attackPatterns = new List<IAttackPattern>();

        private NobilityAnimationController _animationController;
        private Coroutine _attackCoroutine;
        private Player _player;

        private bool _enabled;

        [SerializeField]
        private NobilityAttackPatternProfile attackPatternProfile;

        [field: SerializeField]
        public EntityPosition Position { get; private set; }

        private void Awake()
        {
            if (!attackPatternProfile)
            {
                throw new UnassignedReferenceException("Attack pattern profile is not assigned");
            }
            _animationController = GetComponent<NobilityAnimationController>();
            _attackPatterns.Add(new AttackPattern1(this, _animationController));
            _attackPatterns.Add(new AttackPattern2(this, _animationController));
            _attackPatterns.Add(new AttackPattern3(this, _animationController));
            _attackPatterns.Add(new AttackPattern4(this, _animationController));
            _attackPatterns.Add(new AttackPattern5(this, _animationController));
            _attackPatterns.Add(new AttackPattern6(this, _animationController));

            _player = FindFirstObjectByType<Player>();
            if (_player == null)
            {
                throw new NullReferenceException("Cannot find any player component");
            }
            _enabled = true;
        }

        private void Start()
        {
            Position = EntityPosition.Bottom;
            EntityPositionHandler.Instance.SetPosition(this, EntityPosition.Bottom);
            ProjectileLauncher.Instance.BulletLauncherOrigin = gameObject;
            _attackCoroutine = StartCoroutine(Pattern());
        }

        private void OnDisable()
        {
            _enabled = false;
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
            }
        }

        public IEnumerator MoveOppositePosition()
        {
            EntityPosition targetPosition = Position switch
            {
                EntityPosition.Bottom => EntityPosition.Top,
                EntityPosition.Top => EntityPosition.Bottom,
                _ => throw new InvalidOperationException("Undefined value")
            };
            yield return EntityPositionHandler.Instance.SmoothDampPosition(
                transform,
                targetPosition,
                duration,
                smoothTime);
            Position = targetPosition;
        }

        private IEnumerator Pattern()
        {
            while (_enabled)
            {
                int randomInt = UnityEngine.Random.Range(0, _attackPatterns.Count);
                IAttackPattern attackPattern = _attackPatterns[randomInt];
                yield return attackPattern.GetPatternCoroutine();
                yield return new WaitForSecondsRealtime(term);
            }
        }
    }
}
