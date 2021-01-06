/*
 * @Author: fasthro
 * @Date: 2020/12/30 18:08:16
 * @Description:
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Lockstep
{
    public class EntityService : BaseService, IEntityService
    {
        public GameEntity AddEntity<T>(Contexts contexts, T view) where T : IView
        {
            GameEntity entity = null;
            if (typeof(T) == typeof(IPlayerView))
            {
                entity = CreatePlayer<T>(contexts, view);
            }
            entity.AddCLocalId(view.localID);
            entity.AddCEntityID(ENTITY_ID++);

            var t = view.GetType();
            if (_type2Entities.TryGetValue(t, out var lstObj))
            {
                var lst = lstObj as List<GameEntity>;
                lst.Add(entity);
            }
            else
            {
                var lst = new List<GameEntity>();
                _type2Entities.Add(t, lst);
                lst.Add(entity);
            }

            _id2Entities[entity.cEntityID.id] = entity;

            return entity;
        }

        public List<GameEntity> GetEntities<T>() where T : IView
        {
            var t = typeof(T);
            if (_type2Entities.TryGetValue(t, out var lstObj))
            {
                return lstObj as List<GameEntity>;
            }
            var lst = new List<GameEntity>();
            _type2Entities.Add(t, lst);
            return lst;
        }

        public void RemoveEntity<T>(GameEntity entity) where T : IView
        {
            var t = typeof(T);
            if (_type2Entities.TryGetValue(t, out var lstObj))
            {
                lstObj.Remove(entity);
                _id2Entities.Remove(entity.cEntityID.id);
            }
        }

        static int ENTITY_ID = 0;
        private Dictionary<Type, IList> _type2Entities = new Dictionary<Type, IList>();

        private Dictionary<int, GameEntity> _id2Entities = new Dictionary<int, GameEntity>();

        private GameEntity CreatePlayer<T>(Contexts contexts, T view) where T : IView
        {
            var entity = contexts.game.CreateEntity();

            entity.AddCPosition(view.position);
            entity.AddCRotation(view.rotation);

            entity.AddCView(view);
            view.BindEntity(entity);

            return entity;
        }
    }
}