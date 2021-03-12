// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/30 19:36:55
// * @Description:
// --------------------------------------------------------------------------------

using Entitas.Unity;
using UFramework.Core;

namespace Lockstep.Logic
{
    public class ViewService : BaseGameBehaviour, IViewService, IGameRuntime
    {   
        public T CreateView<T>(string path) where T : IView
        {
            return CreateView<T>(path, -1);
        }

        public T CreateView<T>(string path, int localId) where T : IView
        {
            var gameobject = GammeObjectPool.Instance.Allocate(path, null);
            var view = gameobject.GetComponent<T>();
            var entity = _entityService.AddEntity<T>(LauncherClient.Instance.contexts, view);
            entity.AddCLocalId(localId);
            gameobject.Link(entity);
            return view;
        }

        public void InitGame(GameStartMessage message)
        {
            // 添加玩家
            foreach (var playerData in message.playerDatas)
            {
                var view = _viewService.CreateView<IPlayerView>("Assets/Arts/HeroUnit/Hero_Darius.prefab", playerData.oid);
                // TODO
                if (playerData.uid == 1)
                {
                    _gameService.uid = playerData.uid;
                    _gameService.oid = playerData.oid;
                }
                _playerService.AddPlayer(view.entity, playerData);
            }
        }
    }
}