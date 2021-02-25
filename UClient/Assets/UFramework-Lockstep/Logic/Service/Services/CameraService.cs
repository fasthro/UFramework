// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/25 15:19
// * @Description:
// --------------------------------------------------------------------------------

using UFramework.Core;

namespace Lockstep.Logic
{
    public class CameraService : BaseGameBehaviour, ICameraService
    {
        public override void Initialize()
        {
            Messenger.AddListener(EventDefine.GAME_START, OnGameStart);
        }

        private void OnGameStart()
        {
            if (_playerService.self != null)
                RTSCamera.instance.targetFollow = (_playerService.self.view as BaseGameView)?.gameObject.transform;
        }
    }
}