using System.Collections;

namespace PunchGear.Enemy
{
    public interface IAttackPattern
    {
        public IEnumerator GetPatternCoroutine();
    }
}
