// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/1/4 11:23:52
// * @Description:
// --------------------------------------------------------------------------------

namespace Lockstep.Logic
{
    public class BaseGameService : BaseService
    {
        protected IInitializeService _initializeService;
        protected IInputService _inputService;
        protected ISimulatorService _simulatorService;
        protected IViewService _viewService;
        protected IUIService _uiService;
        protected ICameraService _cameraService;
        protected IVirtualJoyService _virtualJoyService;

        public override void SetReference()
        {
            base.SetReference();

            _initializeService = _container.GetService<IInitializeService>();
            _inputService = _container.GetService<IInputService>();
            _simulatorService = _container.GetService<ISimulatorService>();
            _viewService = _container.GetService<IViewService>();
            _cameraService = _container.GetService<ICameraService>();
            _virtualJoyService = _container.GetService<IVirtualJoyService>();
            _uiService = _container.GetService<IUIService>();
        }
    }
}