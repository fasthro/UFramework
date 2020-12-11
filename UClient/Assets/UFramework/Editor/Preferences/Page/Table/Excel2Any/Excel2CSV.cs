/*
 * @Author: fasthro
 * @Date: 2019-12-17 20:45:12
 * @Description: excel 2 csv
 */
using System;
using System.Collections.Generic;
using System.Text;
using UFramework.Core;

namespace UFramework.Editor.Preferences.Table
{
    public class Excel2CSV : Excel2Any
    {
        private StringBuilder _sb = new StringBuilder();

        public Excel2CSV(ExcelReader reader) : base(reader)
        {
            _reader = reader;
            _sb.Clear();

            // fields
            _sb.Clear();
            for (int i = 0; i < reader.fields.Count; i++)
            {
                if (i == reader.fields.Count - 1) { _sb.Append(reader.fields[i]); }
                else { _sb.Append(reader.fields[i] + ","); }
            }
            _sb.Append("\r\n");

            // data
            for (int i = 0; i < reader.rows.Count; i++)
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                for (int k = 0; k < reader.fields.Count; k++)
                {
                    if (k == reader.fields.Count - 1) { _sb.Append(WrapContext(reader.rows[i].datas[k], reader.types[k])); }
                    else { _sb.Append(WrapContext(reader.rows[i].datas[k], reader.types[k]) + ","); }
                }
                _sb.Append("\r\n");
            }
            IOPath.FileCreateText(reader.options.dataOutFilePath, _sb.ToString());
        }

        private string WrapContext(string content, TableFieldType type)
        {
            if (string.IsNullOrEmpty(content))
            {
                switch (type)
                {
                    case TableFieldType.Byte:
                    case TableFieldType.Int:
                    case TableFieldType.Long:
                    case TableFieldType.Float:
                    case TableFieldType.Double:
                    case TableFieldType.Boolean:
                        return "0";
                    default:
                        return "\"\"";
                }
            }
            else
            {
                switch (type)
                {
                    case TableFieldType.Byte:
                    case TableFieldType.Int:
                    case TableFieldType.Long:
                    case TableFieldType.Float:
                    case TableFieldType.Double:
                    case TableFieldType.Boolean:
                        return content;
                    case TableFieldType.Language:
                        return WrapLanguageContext(content);
                    case TableFieldType.ArrayLanguage:
                        return WrapArrayLanguageContext(content);
                    default:
                        return string.Format("\"{0}\"", content);
                }
            }
        }

        private string WrapLanguageContext(string content)
        {
            char[] separator = new char[] { ':' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (datas.Length == 2)
            {
                var language = typeof(UFramework.Automatic.Lang);
                var modelField = language.GetField(datas[0]);
                var keyField = language.GetField(string.Format("{0}_{1}", datas[0], datas[1]));

                if (modelField != null && keyField != null)
                {
                    return string.Format("{0}:{1}", (int)modelField.GetValue(null), (int)keyField.GetValue(null));
                }
                else
                {
                    Logger.Error("[" + _reader.options.tableName + "] table not find language: " + datas[0] + ":" + datas[1]);
                }
            }
            else
            {
                Logger.Error("[" + _reader.options.tableName + "] table language format error!");
            }

            return "0:0";
        }

        private string WrapArrayLanguageContext(string content)
        {
            char[] separator = new char[] { ',' };
            string[] datas = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            string result = "";
            for (int i = 0; i < datas.Length; i++)
            {
                result += WrapLanguageContext(datas[i]);
                if (i != datas.Length - 1)
                {
                    result += ",";
                }
            }
            return string.Format("\"{0}\"", result);
        }
    }
}