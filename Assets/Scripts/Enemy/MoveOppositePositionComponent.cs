using System.Collections;

namespace PunchGear.Enemy
{
    public class MoveOppositePositionComponent : IAttackPatternComponent
    {
        private readonly EnemyPattern _enemyObject;

        public MoveOppositePositionComponent(EnemyPattern enemyObject)
        {
            _enemyObject = enemyObject;
        }

        public IEnumerator GetPatternComponentCoroutine()
        {
            return _enemyObject.MoveOppositePosition();
        }
    }
}
