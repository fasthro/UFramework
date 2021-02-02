// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace UFramework
{
    public interface IDataManager : IManager
    {
        void Connect(string url);

        void OpenDB(string dbName);

        bool Add<T>(string tabName, T doc);

        T Get<T>(string tabName, string strKey, Expression<Func<T, bool>> filter);

        void Set<T>(string tabName, UpdateDefinition<T> update, Expression<Func<T, bool>> filter);

        T GetDoc<T>(string tabName, Expression<Func<T, bool>> filter);

        List<T> Query<T>(string tabName, Expression<Func<T, bool>> filter = null);

        T Exist<T>(string tabName, Expression<Func<T, bool>> filter);

        void DropDB(string dbName);
    }
}