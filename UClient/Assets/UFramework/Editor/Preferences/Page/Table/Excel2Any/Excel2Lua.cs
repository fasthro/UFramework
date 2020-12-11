/*
 * @Author: fasthro
 * @Date: 2019-12-17 20:45:12
 * @Description: excel 2 lua code
 */
 
using System.Text;
using UFramework.Core;
using UnityEngine;

namespace UFramework.Editor.Preferences.Table
{
    public class Excel2Lua : Excel2Any
    {
        static string Template = @"-- FastEngine
-- excel2table auto generate
-- $descriptions$
local datas = {
$line$
}
return datas
";
        private StringBuilder _sb = new StringBuilder();
        private StringBuilder _wsb = new StringBuilder();
        public Excel2Lua(ExcelReader reader) : base(reader)
        {
            // descriptions
            _sb.Clear();
            for (int i = 0; i < reader.descriptions.Count; i++)
            {
                _sb.Append(reader.descriptions[i] + ",");
            }
            Template = Template.Replace("$descriptions$", _sb.ToString());

            // data0
            _sb.Clear();
            for (int i = 0; i < reader.rows.Count; i++)
            {
                _sb.Append("\t[" + i + "] = {");
                for (int k = 0; k < reader.fields.Count; k++)
                {
                    if (k == reader.fields.Count - 1) { _sb.Append(string.Format("{0} = {1} ", reader.fields[k], WrapContext(reader.rows[i].datas[k], reader.types[k]))); }
                    else { _sb.Append(string.Format("{0} = {1}, ", reader.fields[k], WrapContext(reader.rows[i].datas[k], reader.types[k]))); }
                }
                if (i == reader.rows.Count - 1) { _sb.Append("}\r\n"); }
                else { _sb.Append("},\r\n"); }
            }
            Template = Template.Replace("$line$", _sb.ToString());
            IOPath.FileCreateText(reader.options.luaOutFilePath, Template);
        }

        private string WrapContext(string context, TableFieldType type)
        {
            switch (type)
            {
                case TableFieldType.Boolean:
                    return TableTypeUtils.ContentToBooleanValue(context) ? "true" : "false";

                case TableFieldType.String:
                    return string.Format("\"{0}\"", context);

                case TableFieldType.Vector2:
                    Vector2 v2 = TableTypeUtils.ContentToVector2Value(context);
                    return string.Format("Vector2.New({0}, {1})", v2.x, v2.y);

                case TableFieldType.Vector3:
                    Vector3 v3 = TableTypeUtils.ContentToVector3Value(context);
                    return string.Format("Vector3.New({0}, {1}, {2})", v3.x, v3.y, v3.z);

                case TableFieldType.ArrayByte:
                case TableFieldType.ArrayInt:
                case TableFieldType.ArrayLong:
                case TableFieldType.ArrayFloat:
                case TableFieldType.ArrayDouble:
                    return "{" + context + "}";

                case TableFieldType.ArrayBoolean:
                    _wsb.Clear();
                    _wsb.Append("{");
                    bool[] bools = TableTypeUtils.ContentToArrayBooleanValue(context);
                    for (int i = 0; i < bools.Length; i++)
                    {
                        var bs = bools[i] ? "true" : "false";
                        if (i == bools.Length - 1) { _wsb.Append(bs); }
                        else { _wsb.Append(bs + ", "); }
                    }
                    _wsb.Append("}");
                    return _wsb.ToString();
                case TableFieldType.ArrayString:
                    _wsb.Clear();
                    _wsb.Append("{");
                    string[] strs = TableTypeUtils.ContentToArrayStringValue(context);
                    for (int i = 0; i < strs.Length; i++)
                    {
                        var str = string.Format("\"{0}\"", strs[i]);
                        if (i == strs.Length - 1) { _wsb.Append(str); }
                        else { _wsb.Append(str + ", "); }
                    }
                    _wsb.Append("}");
                    return _wsb.ToString();

                case TableFieldType.ArrayVector2:
                    _wsb.Clear();
                    _wsb.Append("{");
                    Vector2[] v2s = TableTypeUtils.ContentToArrayVector2Value(context);
                    for (int i = 0; i < v2s.Length; i++)
                    {
                        var v2str = string.Format("Vector2.New({0}, {1})", v2s[i].x, v2s[i].y);
                        if (i == v2s.Length - 1) { _wsb.Append(v2str); }
                        else { _wsb.Append(v2str + ", "); }
                    }
                    _wsb.Append("}");
                    return _wsb.ToString();

                case TableFieldType.ArrayVector3:
                    _wsb.Clear();
                    _wsb.Append("{");
                    Vector3[] v3s = TableTypeUtils.ContentToArrayVector3Value(context);
                    for (int i = 0; i < v3s.Length; i++)
                    {
                        var v3str = string.Format("Vector3.New({0}, {1}, {2})", v3s[i].x, v3s[i].y, v3s[i].z);
                        if (i == v3s.Length - 1) { _wsb.Append(v3str); }
                        else { _wsb.Append(v3str + ", "); }
                    }
                    _wsb.Append("}");
                    return _wsb.ToString();

                default:
                    return context;
            }
        }
    }
}