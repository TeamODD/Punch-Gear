using System.Collections;
using PunchGear.Entity;
using UnityEngine;

namespace PunchGear.Enemy
{
    public class AttackPattern6 : IAttackPattern
    {
        private readonly EnemyPattern _enemyPattern;

        public AttackPattern6(EnemyPattern enemyPattern)
        {
            _enemyPattern = enemyPattern;
        }

        public IEnumerator GetPatternCoroutine()
        {
            ProjectileLauncher launcher = ProjectileLauncher.Instance;
            yield return new WaitForSecondsRealtime(_enemyPattern.normal);
            yield return launcher.Launch(_enemyPattern.slow);
            yield return _enemyPattern.JoinCoroutines(
                launcher.Launch(0),
                _enemyPattern.MoveOppositePosition(),
                new WaitForSecondsRealtime(_enemyPattern.normal));
            yield return _enemyPattern.JoinCoroutines(
                _enemyPattern.MoveOppositePosition(),
                new WaitForSecondsRealtime(_enemyPattern.normal));
            yield return new WaitForSecondsRealtime(_enemyPattern.fast);
            yield return launcher.Launch(_enemyPattern.fast);
        }
    }
}
