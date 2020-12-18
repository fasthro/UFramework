﻿/*
 * @Author: fasthro
 * @Date: 2020/12/18 15:28:02
 * @Description: 数据库数据管理
 */

using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LockstepServer
{
    public class DataManager : BaseManager
    {
        private MongoHelper _mongoHelper = null;

        #region base

        protected override void OnInitialize()
        {
            Connect(GameConst.DB_URL);
            OpenDB(GameConst.DB_NAME);
        }

        protected override void OnDispose()
        {
            _mongoHelper = null;
        }

        #endregion base

        #region db api

        public void Connect(string url)
        {
            _mongoHelper = new MongoHelper(url);
        }

        /// <summary>
        /// 打开数据库
        /// </summary>
        public void OpenDB(string dbName)
        {
            _mongoHelper.OpenDB(dbName);
        }

        /// <summary>
        /// 添加一行
        /// </summary>
        public bool Add<T>(string tabName, T doc)
        {
            return _mongoHelper.Insert<T>(tabName, doc);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public T Get<T>(string tabName, string strKey, Expression<Func<T, bool>> filter)
        {
            var projection = Builders<T>.Projection.Include(strKey);
            return _mongoHelper.SelectOne<T>(tabName, projection, filter);
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        public void Set<T>(string tabName, UpdateDefinition<T> update, Expression<Func<T, bool>> filter)
        {
            _mongoHelper.UpdateOne<T>(tabName, update, filter);
        }

        /// <summary>
        /// 获取一行
        /// </summary>
        public T GetDoc<T>(string tabName, Expression<Func<T, bool>> filter)
        {
            return _mongoHelper.SelectOne<T>(tabName, filter);
        }

        /// <summary>
        /// 组合查询
        /// </summary>
        public List<T> Query<T>(string tabName, Expression<Func<T, bool>> filter = null)
        {
            return _mongoHelper.Select<T>(tabName, filter);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        public T Exist<T>(string tabName, Expression<Func<T, bool>> filter)
        {
            return Get<T>(tabName, "uid", filter);
        }

        /// <summary>
        /// 删除数据库
        /// </summary>
        public void DropDB(string dbName)
        {
            _mongoHelper.DropDB(dbName);
        }

        #endregion db api
    }
}