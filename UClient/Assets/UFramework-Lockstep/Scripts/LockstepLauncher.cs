/*
 * @Author: fasthro
 * @Date: 2020-12-15 14:35:37
 * @Description: Lockstep Launcher
 */

using LSC;

namespace UFramework.Lockstep
{
    [MonoSingletonPath("UFramework-Lockstep")]
    public class LockstepLauncher : MonoSingleton<LockstepLauncher>
    {
        protected override void OnSingletonAwake()
        {
            #region LSTime

            LSTime.Initialize();
            Service.Instance.AddUpdateListener(LSTime.Update, ServiceUpdateOrder.Before);

            #endregion

            Service.Instance.RegisterService(new GameService());
        }

        public void Launched()
        {
            Service.Instance.container.GetService<GameService>().Launch();
        }
    }
}
