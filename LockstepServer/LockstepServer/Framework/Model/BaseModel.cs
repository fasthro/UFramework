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
    public class BaseModel : BaseBehaviour
    {
        public BaseModel(string tabName) : base()
        {
            _tableName = tabName;
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
            return _dataService.GetDoc(tableName, filter);
        }

        /// <summary>
        /// 查询结果集
        /// </summary>
        public List<T> Query<T>(Expression<Func<T, bool>> filter = null)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new Exception();
            }
            return _dataService.Query(tableName, filter);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        public T Exist<T>(Expression<Func<T, bool>> filter)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new Exception();
            }
            return _dataService.Exist<T>(tableName, filter);
        }

        /// <summary>
        /// 添加
        /// </summary>
        protected bool Add<T>(T doc)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new Exception();
            }
            return _dataService.Add(tableName, doc);
        }

        /// <summary>
        /// 获取
        /// </summary>
        protected T Get<T>(string strKey, Expression<Func<T, bool>> filter)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new Exception();
            }
            return _dataService.Get<T>(tableName, strKey, filter);
        }

        /// <summary>
        /// 设置
        /// </summary>
        protected void Set<T>(UpdateDefinition<T> update, Expression<Func<T, bool>> filter)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new Exception();
            }
            _dataService.Set(tableName, update, filter);
        }

        private string _tableName;
    }
}