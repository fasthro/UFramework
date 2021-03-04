// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-07-01 20:01:09
// * @Description:
// --------------------------------------------------------------------------------

using System.Text;

namespace UFramework.Editor.Preferences.Localization
{
    public class ExcelSheet
    {
        public string name;
        public ExcelColumn[] columns;

        public string ToLuaKeyString()
        {
            var builder = new StringBuilder();
            for (var i = 1; i < columns.Length; i++)
            {
                builder.AppendLine($"\t\t{name}_{columns[i].key} = {i - 1},");
            }

            return builder.ToString().Trim('\r', '\n');
        }

        public string ToCSharpKeyString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 1; i < columns.Length; i++)
            {
                builder.AppendLine($"\t\tpublic static int {name}_{columns[i].key} = {i - 1};");
            }

            return builder.ToString().Trim('\r', '\n');
        }

        public string ToValueString(int index)
        {
            var builder = new StringBuilder();
            for (var i = 1; i < columns.Length; i++)
            {
                builder.Append(columns[i].values[index] + "\n");
            }

            return builder.ToString().Trim('\r', '\n');
        }

        public override string ToString()
        {
            return "sheet name: " + name + " column count: " + columns.Length;
        }
    }
}