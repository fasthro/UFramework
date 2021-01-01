/*
 * @Author: fasthro
 * @Date: 2020/12/18 15:22:03
 * @Description:
 */

using System;
namespace LockstepServer.Src
{
    public class BaseGameService : BaseService
    {
        public override void SetReference()
        {
            _playerService = _container.GetService<IPlayerService>();
            _roomService = _container.GetService<IRoomService>();
        }

        protected IPlayerService _playerService;
        protected IRoomService _roomService;
    }
}
