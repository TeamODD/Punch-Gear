using System.Collections;
using PunchGear.Entity;

namespace PunchGear.Enemy
{
    public class AttackPattern2 : IAttackPattern
    {
        private readonly EnemyPattern _enemyPattern;
        private readonly float _projectileFastSpeed;
        private readonly float _projectileSlowSpeed;

        public AttackPattern2(EnemyPattern enemyPattern, float projectileFastSpeed, float projectileSlowSpeed)
        {
            _enemyPattern = enemyPattern;
            _projectileFastSpeed = projectileFastSpeed;
            _projectileSlowSpeed = projectileSlowSpeed;
        }

        public IEnumerator GetPatternCoroutine()
        {
            ProjectileLauncher launcher = ProjectileLauncher.Instance;
            yield return _enemyPattern.StartCoroutine(launcher.Launch(_projectileFastSpeed));
            yield return _enemyPattern.StartCoroutine(launcher.Launch(_projectileFastSpeed));
            yield return _enemyPattern.StartCoroutine(launcher.Launch(_projectileFastSpeed));
            yield return _enemyPattern.StartCoroutine(launcher.Launch(_projectileFastSpeed));
            yield return _enemyPattern.StartCoroutine(_enemyPattern.MoveOppositePosition());
            yield return _enemyPattern.StartCoroutine(launcher.Launch(_projectileSlowSpeed));
            yield return _enemyPattern.StartCoroutine(launcher.Launch(_projectileSlowSpeed));
        }
    }
}
