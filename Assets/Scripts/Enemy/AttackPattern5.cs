using UnityEngine;
using System.Collections;
using PunchGear.Entity;
using System.Collections.Generic;
using System;

namespace PunchGear.Enemy
{
    public class AttackPattern5 : IAttackPattern
    {
        private readonly EnemyPattern _enemyPattern;
        private readonly NobilityAnimationController _animationController;
        private List<Func<IEnumerator>> _actions;

        public AttackPattern5(EnemyPattern enemyPattern, NobilityAnimationController animationController)
        {
            _enemyPattern = enemyPattern;
            _animationController = animationController;
            _actions = new List<Func<IEnumerator>>
            {
                () => _enemyPattern.JoinCoroutines(
                    _enemyPattern.MoveOppositePosition(),
                    _animationController.TransitAnimationRoutine(),
                    new WaitForSecondsRealtime(_enemyPattern.fast)),
                () =>
                {
                    ProjectileLauncher launcher = ProjectileLauncher.Instance;
                    return _enemyPattern.JoinCoroutines(launcher.Launch(_enemyPattern.fast), _animationController.TransitAnimationRoutine());
                },
                () =>
                {
                    ProjectileLauncher launcher = ProjectileLauncher.Instance;
                    return _enemyPattern.JoinCoroutines(launcher.Launch(_enemyPattern.fast), _animationController.TransitAnimationRoutine());
                },
                () =>
                {
                    ProjectileLauncher launcher = ProjectileLauncher.Instance;
                    return _enemyPattern.JoinCoroutines(launcher.Launch(0), _animationController.TransitAnimationRoutine());
                }
            };
        }

        public IEnumerator GetPatternCoroutine()
        {
            ProjectileLauncher launcher = ProjectileLauncher.Instance;
            for (int i = 0; i < 5; i++)
            {
                int index = UnityEngine.Random.Range(0, _actions.Count);
                Func<IEnumerator> routine = _actions[index];
                yield return routine();
            }
        }
    }
}
