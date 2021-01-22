/*
 * @Author: fasthro
 * @Date: 2020/12/30 18:08:16
 * @Description:
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lockstep
{
    public class EntityService : BaseService, IEntityService
    {
        /// <summary>
        /// 实体ID
        /// </summary>
        private static int ENTITY_ID = 0;

        /// <summary>
        /// 类型实体列表字典
        /// </summary>
        private Dictionary<Type, IList> _type2EntityDict = new Dictionary<Type, IList>();

        /// <summary>
        /// id实体列表字典
        /// </summary>
        private Dictionary<int, GameEntity> _id2EntityDict = new Dictionary<int, GameEntity>();

        public GameEntity AddEntity<T>(Contexts contexts, T view) where T : IView
        {
            GameEntity entity = null;
            if (typeof(T) == typeof(IPlayerView))
                entity = CreatePlayer<T>(contexts, view);

            if (entity == null)
                return null;

            entity.AddCEntityID(ENTITY_ID++);

            var ts = view.GetType().FindInterfaces((type, criteria) =>
                    type.GetInterfaces().Any(t => t == typeof(IView)), view)
                .ToArray();
            foreach (var t in ts)
            {
                if (_type2EntityDict.TryGetValue(t, out var lstObj))
                {
                    if (lstObj is List<GameEntity> lst)
                        lst.Add(entity);
                }
                else
                {
                    var lst = new List<GameEntity>();
                    _type2EntityDict.Add(t, lst);
                    lst.Add(entity);
                }
            }

            _id2EntityDict[entity.cEntityID.id] = entity;

            return entity;
        }

        public List<GameEntity> GetEntities<T>() where T : IView
        {
            var t = typeof(T);
            if (_type2EntityDict.TryGetValue(t, out var lstObj))
            {
                return lstObj as List<GameEntity>;
            }

            var lst = new List<GameEntity>();
            _type2EntityDict.Add(t, lst);
            return lst;
        }

        public void RemoveEntity<T>(GameEntity entity) where T : IView
        {
            var t = typeof(T);
            if (_type2EntityDict.TryGetValue(t, out var lstObj))
            {
                lstObj.Remove(entity);
                _id2EntityDict.Remove(entity.cEntityID.id);
            }
        }

        private GameEntity CreatePlayer<T>(Contexts contexts, T view) where T : IView
        {
            var entity = contexts.game.CreateEntity();
            entity.AddCPosition(LSVector3.zero);
            entity.AddCSpeed(Fix64.One);
            entity.AddCMovement(LSVector3.zero);

            entity.AddCView(view);
            view.BindEntity(entity);

            return entity;
        }
    }
}