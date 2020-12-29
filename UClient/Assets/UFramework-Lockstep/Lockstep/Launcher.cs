/*
 * @Author: fasthro
 * @Date: 2020/12/29 20:10:33
 * @Description:
 */

namespace Lockstep
{
    public class Launcher : IBehaviour
    {
        public static Launcher Instance => _instance;

        public ServiceContainer serviceContainer { get; private set; }

        public Contexts contexts { get; private set; }

        public LockstepSystem system { get; private set; }

        public static Launcher Create()
        {
            if (_instance == null)
            {
                _instance = new Launcher();
            }
            return _instance;
        }

        public void Initialize()
        {
            serviceContainer = new ServiceContainer();
            serviceContainer.RegisterService(new GameService());

            contexts = Contexts.sharedInstance;
            system = new LockstepSystem(contexts);
        }

        public void Update()
        {
            system.Execute();
            system.Cleanup();
        }

        public void Dispose()
        {
            system.TearDown();
        }

        private static Launcher _instance;

        private Launcher()
        {
        }
    }
}