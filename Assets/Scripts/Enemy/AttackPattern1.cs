using System.Collections;
using PunchGear.Entity;

namespace PunchGear.Enemy
{
    public class AttackPattern1 : IAttackPattern
    {
        private readonly EnemyPattern _enemyPattern;
        private readonly float _projectileSpeed;

        public AttackPattern1(EnemyPattern enemyPattern, float projectileSpeed)
        {
            _enemyPattern = enemyPattern;
            _projectileSpeed = projectileSpeed;
        }

        public IEnumerator GetPatternCoroutine()
        {
            ProjectileLauncher launcher = ProjectileLauncher.Instance;
            yield return _enemyPattern.StartCoroutine(launcher.Launch(_projectileSpeed));
            yield return _enemyPattern.StartCoroutine(_enemyPattern.MoveOppositePosition());
            yield return _enemyPattern.StartCoroutine(launcher.Launch(_projectileSpeed));
            yield return _enemyPattern.StartCoroutine(_enemyPattern.MoveOppositePosition());
            yield return _enemyPattern.StartCoroutine(launcher.Launch(_projectileSpeed));
        }
    }
}
