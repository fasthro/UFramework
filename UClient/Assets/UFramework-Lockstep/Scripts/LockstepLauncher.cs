/*
 * @Author: fasthro
 * @Date: 2020-12-15 14:35:37
 * @Description: Lockstep Launcher
 */

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
            _launcher = Lockstep.Launcher.Create();
        }

        protected override void OnSingletonStart()
        {
            _launcher.Initialize();
        }

        protected override void OnSingletonUpdate(float deltaTime)
        {
            _launcher.Update();
        }

        protected override void OnSingletonDestory()
        {
            _launcher.Dispose();
        }

        private Lockstep.Launcher _launcher;
    }
}