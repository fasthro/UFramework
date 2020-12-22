/*
 * @Author: fasthro
 * @Date: 2020/12/18 14:35:40
 * @Description:
 */

using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LockstepServer
{
    public class BaseModel
    {
        protected static DataManager dataManager;

        private string _tableName;

        public BaseModel(string tabName)
        {
            _tableName = tabName;

            if (dataManager == null)
            {
                dataManager = Service.Instance.GetManager<DataManager>();
            }
        }

        public string tableName
        {
            get { return _tableName; }
        }

        /// <summary>
        /// 获取文档
        /// </summary>
        public T GetDoc<T>(Expression<Func<T, bool>> filter)
        {
            return dataManager.GetDoc(tableName, filter);
        }

        /// <summary>
        /// 查询结果集
        /// </summary>
        public List<T> Query<T>(Expression<Func<T, bool>> filter = null)
        {
            if (string.IsNullOrEmpty(tableName) || dataManager == null)
            {
                throw new Exception();
            }
            return dataManager.Query(tableName, filter);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        public T Exist<T>(Expression<Func<T, bool>> filter)
        {
            if (string.IsNullOrEmpty(tableName) || dataManager == null)
            {
                throw new Exception();
            }
            return dataManager.Exist<T>(tableName, filter);
        }

        /// <summary>
        /// 添加
        /// </summary>
        protected bool Add<T>(T doc)
        {
            if (string.IsNullOrEmpty(tableName) || dataManager == null)
            {
                throw new Exception();
            }
            return dataManager.Add(tableName, doc);
        }

        /// <summary>
        /// 获取
        /// </summary>
        protected T Get<T>(string strKey, Expression<Func<T, bool>> filter)
        {
            if (string.IsNullOrEmpty(tableName) || dataManager == null)
            {
                throw new Exception();
            }
            return dataManager.Get<T>(tableName, strKey, filter);
        }

        /// <summary>
        /// 设置
        /// </summary>
        protected void Set<T>(UpdateDefinition<T> update, Expression<Func<T, bool>> filter)
        {
            if (string.IsNullOrEmpty(tableName) || dataManager == null)
            {
                throw new Exception();
            }
            dataManager.Set(tableName, update, filter);
        }
    }
}