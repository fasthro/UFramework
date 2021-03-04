// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-06-29 11:26:04
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;
using UFramework.Core;

namespace UFramework.Editor.Preferences.Table
{
    public class ExcelReaderRow
    {
        public List<string> descriptions; // 描述
        public List<string> fields; // 字段
        public List<TableFieldType> types; // 类型
        public List<string> datas; // 数据

        public ExcelReaderRow()
        {
            descriptions = new List<string>();
            fields = new List<string>();
            types = new List<TableFieldType>();
            datas = new List<string>();
        }
    }
}