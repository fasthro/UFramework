/*
 * @Author: fasthro
 * @Date: 2020-01-04 17:52:23
 * @Description: excel 2 lua index
 */
using System.Text;
using UnityEngine;

namespace UFramework.Language
{
    public class Excel2LuaIndex
    {
        private StringBuilder _builder = new StringBuilder();

        public Excel2LuaIndex(ExcelReader reader)
        {
            _builder.Clear();

            _builder.AppendLine("--[[ aotu generated]]");
            _builder.AppendLine("--[[");
            _builder.AppendLine(" * @Author: fasthro");
            _builder.AppendLine(" * @Description: i18 model & key");
            _builder.AppendLine(" ]]");

            // model
            _builder.AppendLine("language_model = {");
            for (int i = 0; i < reader.sheets.Count; i++)
            {
                _builder.AppendLine(string.Format("\t{0} = {1},", reader.sheets[i].name, i));
            }
            _builder.AppendLine("}");

            // key
            _builder.AppendLine("language_key = {");

            for (int i = 0; i < reader.sheets.Count; i++)
            {
                _builder.AppendLine(reader.sheets[i].ToLuaKeyString());
            }
            _builder.AppendLine("}");
            
            // TODO
            // FilePathUtils.FileWriteAllText(FilePathUtils.Combine(Application.dataPath, "LuaScripts/language.lua"), m_builder.ToString());
        }
    }
}