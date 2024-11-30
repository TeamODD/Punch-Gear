using PunchGear.Attributes;
using PunchGear.Enemy;

using UnityEngine;

namespace PunchGear.Entity
{
    [RequireComponent(typeof(NobilityAnimationController))]
    [RequireComponent(typeof(EnemyPattern))]
    public class Nobility : HealthHolderBase
    {
        [ReadOnlyField]
        [SerializeField]
        private bool componentEnabled;

        private NobilityAnimationController _animationController;
        private EnemyPattern _enemyPattern;

        private void Awake()
        {
            _animationController = GetComponent<NobilityAnimationController>();
            _enemyPattern = GetComponent<EnemyPattern>();
        }

        private void OnEnable()
        {
            componentEnabled = true;
        }

        private void OnDisable()
        {
            componentEnabled = false;
        }
    }
}
