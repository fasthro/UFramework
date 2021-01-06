/*
 * @Author: fasthro
 * @Date: 2020/12/30 19:36:55
 * @Description:
 */

using Entitas.Unity;
using UFramework.Core;
using UnityEngine;

namespace Lockstep.Logic
{
    public class ViewService : BaseGameBehaviour, IViewService
    {
        public T CreateView<T>(string path) where T : IView
        {
            return CreateView<T>(path, -1);
        }

        public T CreateView<T>(string path, int localId) where T : IView
        {
            var gameobject = GammeObjectPool.Instance.Allocate(path, null);
            var view = gameobject.GetComponent<T>();
            view.localID = localId;
            var entity = _entityService.AddEntity<T>(LauncherClient.Instance.contexts, view);
            gameobject.Link(entity);
            return view;
        }
    }
}