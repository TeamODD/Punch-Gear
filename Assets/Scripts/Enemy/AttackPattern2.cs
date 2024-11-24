using System.Collections;
using PunchGear.Entity;
using UnityEngine;

namespace PunchGear.Enemy
{
    public class AttackPattern2 : IAttackPattern
    {
        private readonly EnemyPattern _enemyPattern;
        private readonly NobilityAnimationController _animationController;

        public AttackPattern2(EnemyPattern enemyPattern, NobilityAnimationController animationController)
        {
            _enemyPattern = enemyPattern;
            _animationController = animationController;
        }

        public IEnumerator GetPatternCoroutine()
        {
            ProjectileLauncher launcher = ProjectileLauncher.Instance;
            yield return new WaitForSecondsRealtime(_enemyPattern.fast);
            yield return _enemyPattern.JoinCoroutines(
                launcher.Launch(0),
                _animationController.TransitAnimationRoutine(),
                new WaitForSecondsRealtime(_enemyPattern.fast));
            yield return _enemyPattern.JoinCoroutines(
                launcher.Launch(0),
                _animationController.TransitAnimationRoutine(),
                new WaitForSecondsRealtime(_enemyPattern.fast));
            yield return _enemyPattern.JoinCoroutines(
                launcher.Launch(0),
                _animationController.TransitAnimationRoutine(),
                new WaitForSecondsRealtime(_enemyPattern.fast));
            yield return _enemyPattern.JoinCoroutines(
                launcher.Launch(0),
                _animationController.TransitAnimationRoutine(),
                _enemyPattern.MoveOppositePosition(),
                new WaitForSecondsRealtime(_enemyPattern.normal));
            yield return new WaitForSecondsRealtime(_enemyPattern.slow);
            yield return _enemyPattern.JoinCoroutines(
                launcher.Launch(0),
                _animationController.TransitAnimationRoutine(),
                new WaitForSecondsRealtime(_enemyPattern.slow));
            yield return _enemyPattern.JoinCoroutines(
                launcher.Launch(0),
                _animationController.TransitAnimationRoutine());
        }
    }
}
