using System.Collections;
using PunchGear.Entity;
using UnityEngine;

namespace PunchGear.Enemy
{
    public class AttackPattern1 : IAttackPattern
    {
        private readonly EnemyPattern _enemyPattern;

        public AttackPattern1(EnemyPattern enemyPattern)
        {
            _enemyPattern = enemyPattern;
        }

        public IEnumerator GetPatternCoroutine()
        {
            ProjectileLauncher launcher = ProjectileLauncher.Instance;
            yield return new WaitForSecondsRealtime(_enemyPattern.normal);
            yield return _enemyPattern.JoinCoroutines(
                launcher.Launch(0),
                _enemyPattern.MoveOppositePosition(),
                new WaitForSecondsRealtime(_enemyPattern.normal));
            yield return new WaitForSecondsRealtime(_enemyPattern.normal);
            yield return _enemyPattern.JoinCoroutines(
                launcher.Launch(0),
                _enemyPattern.MoveOppositePosition(),
                new WaitForSecondsRealtime(_enemyPattern.normal));
            yield return new WaitForSecondsRealtime(_enemyPattern.normal);
            yield return _enemyPattern.StartCoroutine(launcher.Launch(_enemyPattern.fast));
        }
    }
}
