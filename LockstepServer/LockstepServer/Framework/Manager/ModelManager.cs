/*
 * @Author: fasthro
 * @Date: 2020/12/18 15:41:40
 * @Description: 数据库模型管理
 */

using System;
using System.Collections.Generic;

namespace LockstepServer
{
    public class ModelManager : BaseManager
    {
        private Dictionary<Type, BaseModel> _modelDict = new Dictionary<Type, BaseModel>();

        public T GetModel<T>() where T : BaseModel
        {
            var key = typeof(T);
            if (_modelDict.ContainsKey(key))
            {
                return _modelDict[key] as T;
            }
            return default(T);
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

        protected override void OnInitialize()
        {
            _modelDict.Clear();
        }
    }
}