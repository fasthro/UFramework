/*
 * @Author: fasthro
 * @Date: 2019-12-17 20:45:12
 * @Description: excel 2 csv
 */
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UFramework.Table
{
    public class Excel2CSV : Excel2Any
    {
        private StringBuilder m_stringBuilder = new StringBuilder();
        private ExcelReader m_reader;

        public Excel2CSV(ExcelReader reader) : base(reader)
        {
            m_reader = reader;
            m_stringBuilder.Clear();

            // fields
            m_stringBuilder.Clear();
            for (int i = 0; i < reader.fields.Count; i++)
            {
                if (i == reader.fields.Count - 1) { m_stringBuilder.Append(reader.fields[i]); }
                else { m_stringBuilder.Append(reader.fields[i] + ","); }
            }
            m_stringBuilder.Append("\r\n");

            // data
            for (int i = 0; i < reader.rows.Count; i++)
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                for (int k = 0; k < reader.fields.Count; k++)
                {
                    if (k == reader.fields.Count - 1) { m_stringBuilder.Append(WrapContext(reader.rows[i].datas[k], reader.types[k])); }
                    else { m_stringBuilder.Append(WrapContext(reader.rows[i].datas[k], reader.types[k]) + ","); }
                }
                m_stringBuilder.Append("\r\n");
            }
            IOPath.FileCreateText(reader.options.dataOutFilePath, m_stringBuilder.ToString());
        }

        private string WrapContext(string content, FieldType type)
        {
            if (string.IsNullOrEmpty(content))
            {
                switch (type)
                {
                    case FieldType.Byte:
                    case FieldType.Int:
                    case FieldType.Long:
                    case FieldType.Float:
                    case FieldType.Double:
                    case FieldType.Boolean:
                        return "0";
                    default:
                        return "\"\"";
                }
            }
            else
            {
                switch (type)
                {
                    case FieldType.Byte:
                    case FieldType.Int:
                    case FieldType.Long:
                    case FieldType.Float:
                    case FieldType.Double:
                    case FieldType.Boolean:
                        return content;
                    case FieldType.Language:
                        return WrapLanguageContext(content);
                    case FieldType.ArrayLanguage:
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
                var language = typeof(UFramework.Automatic.Language);
                var modelField = language.GetField(datas[0]);
                var keyField = language.GetField(string.Format("{0}_{1}", datas[0], datas[1]));

                if (modelField != null && keyField != null)
                {
                    return string.Format("{0}:{1}", (int)modelField.GetValue(null), (int)keyField.GetValue(null));
                }
                else
                {
                    Logger.Error("[" + m_reader.options.tableName + "] table not find language: " + datas[0] + ":" + datas[1]);
                }
            }
            else
            {
                Logger.Error("[" + m_reader.options.tableName + "] table language format error!");
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