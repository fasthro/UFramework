// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-06-29 11:26:04
// * @Description:
// --------------------------------------------------------------------------------

using UFramework.Core;

namespace UFramework.Editor.Preferences.Table
{
    public class ExcelReaderOptions
    {
        public TableFormat outFormatOptions; // 导出格式
        public TableDataIndexFormat dataFormatOptions; // 数据格式
        public string tableName; // 表名称
        public string dataOutDirectory; // 数据文件输出目录
        public string tableModelOutDirectory; // table model 文件输出目录
        public string luaOutDirectory; // lua 文件输出目录


        // 数据文件输出路径
        public string dataOutFilePath
        {
            get
            {
                if (outFormatOptions == TableFormat.CSV) return IOPath.PathCombine(dataOutDirectory, tableName + ".csv");
                else return IOPath.PathCombine(dataOutDirectory, tableName + ".json");
            }
        }

        // table model 文件输出路径
        public string tableModelOutFilePath => IOPath.PathCombine(tableModelOutDirectory, tableName + "Table.cs");

        // lua 文件输出路径
        public string luaOutFilePath => IOPath.PathCombine(luaOutDirectory, tableName + ".lua");
    }
}