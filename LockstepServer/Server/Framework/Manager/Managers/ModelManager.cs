// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace UFramework
{
    public class ModelManager : BaseManager, IModelManager
    {
        private Dictionary<Type, BaseModel> _modelDict = new Dictionary<Type, BaseModel>();

        public T GetModel<T>() where T : BaseModel, new()
        {
            var key = typeof(T);
            if (_modelDict.ContainsKey(key))
            {
                return _modelDict[key] as T;
            }

            AddModel<T>(new T());
            return _modelDict[key] as T;
        }

        public void AddModel<T>(BaseModel model) where T : BaseModel
        {
            var key = typeof(T);
            if (_modelDict.ContainsKey(key))
            {
                throw new Exception();
            }

            _modelDict.Add(key, model);
        }

        public void RemoveModel<T>() where T : BaseModel
        {
            var key = typeof(T);
            if (_modelDict.ContainsKey(key))
            {
                _modelDict.Remove(key);
            }
        }

        public override void Initialize()
        {
            _modelDict.Clear();
        }
    }
}