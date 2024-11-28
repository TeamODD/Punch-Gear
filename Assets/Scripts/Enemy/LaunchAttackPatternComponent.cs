using System.Collections;

using PunchGear.Entity;

namespace PunchGear.Enemy
{
    public class LaunchAttackPatternComponent : IAttackPatternComponent
    {
        private readonly IProjectileLauncher _launcher;

        public LaunchAttackPatternComponent(IProjectileLauncher launcher)
        {
            _launcher = launcher;
        }

        public IEnumerator GetPatternComponentCoroutine()
        {
            yield return _launcher.Launch(0f);
        }
    }
}
