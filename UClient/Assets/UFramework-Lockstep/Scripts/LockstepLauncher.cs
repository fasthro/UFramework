/*
 * @Author: fasthro
 * @Date: 2020-12-15 14:35:37
 * @Description: Lockstep Launcher
 */

using Lockstep.Logic;
using UFramework;

namespace ULockstepFramework
{
    [MonoSingletonPath("UFramework-Lockstep")]
    public class LockstepLauncher : MonoSingleton<LockstepLauncher>
    {
        public void Launched()
        {
        }

        protected override void OnSingletonAwake()
        {
            _launcher = new LauncherClient();
        }

        protected override void OnSingletonStart()
        {
            _launcher.Initialize();
            _launcher.Test();
        }

        protected override void OnSingletonUpdate(float deltaTime)
        {
            _launcher.Update();
        }

        protected override void OnSingletonDestory()
        {
            _launcher.Dispose();
        }

        private LauncherClient _launcher;
    }
}