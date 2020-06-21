/*
 * @Author: fasthro
 * @Date: 2020-05-30 17:18:34
 * @Description: table config
 */

using System.Collections.Generic;
using UFramework.Config;

namespace UFramework.Table
{
    public class TableItem
    {
        // 数据表名称
        public string tableName { get; set; }
        // 数据格式
        public DataFormatOptions dataFormatOptions { get; set; }
    }

    public class TableConfig : IConfigObject
    {
        public string name { get; }

        // 导出格式
        public FormatOptions outFormatOptions = FormatOptions.CSV;
        // 命名空间
        public string tableModelNamespace;
        // 数据表
        public Dictionary<string, TableItem> tableDictionary = new Dictionary<string, TableItem>();
    }
}
