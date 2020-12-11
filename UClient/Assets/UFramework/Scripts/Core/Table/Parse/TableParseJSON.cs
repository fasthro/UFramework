/*
 * @Author: fasthro
 * @Date: 2019-12-19 17:11:35
 * @Description: table parse json
 */
using System.Collections.Generic;

namespace UFramework.Core
{
    public class TableParseJSON : TableParse
    {
        public TableParseJSON(string tableName, TableFormat format) : base(tableName, format)
        {
        }

        public override T[] ParseArray<T>()
        {
            throw new System.NotImplementedException();
        }

        public override Dictionary<int, T> ParseIntDictionary<T>()
        {
            throw new System.NotImplementedException();
        }

        public override Dictionary<string, T> ParseStringDictionary<T>()
        {
            throw new System.NotImplementedException();
        }

        public override Dictionary<int, Dictionary<int, T>> ParseInt2IntDictionary<T>()
        {
            throw new System.NotImplementedException();
        }
    }
}