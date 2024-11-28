using System.Collections;

namespace PunchGear.Enemy
{
    public interface IAttackPatternComponent
    {
        public IEnumerator GetPatternComponentCoroutine();
    }
}
