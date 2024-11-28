using System.Collections;

namespace PunchGear.Entity
{
    public interface IProjectileLauncher : IProjectileEventHandler
    {
        public IEnumerator Launch(float launcherCooldown);
    }
}
