using System.Collections;
using PunchGear.Entity;
using UnityEngine;

namespace PunchGear.Enemy
{
    public class AttackPattern4 : IAttackPattern
    {
        private readonly EnemyPattern _enemyPattern;
        private readonly NobilityAnimationController _animationController;

        public AttackPattern4(EnemyPattern enemyPattern, NobilityAnimationController animationController)
        {
            _enemyPattern = enemyPattern;
            _animationController = animationController;
        }

        public IEnumerator GetPatternCoroutine()
        {
            ProjectileLauncher launcher = ProjectileLauncher.Instance;
            yield return _enemyPattern.JoinCoroutines(
                _enemyPattern.MoveOppositePosition(),
                _animationController.TransitAnimationRoutine(),
                new WaitForSecondsRealtime(_enemyPattern.fast));
            yield return _enemyPattern.JoinCoroutines(
                launcher.Launch(0),
                _animationController.TransitAnimationRoutine(),
                new WaitForSecondsRealtime(_enemyPattern.slow));
            yield return _enemyPattern.JoinCoroutines(launcher.Launch(0), _animationController.TransitAnimationRoutine());
        }
    }
}
