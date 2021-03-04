// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-07-01 20:01:09
// * @Description:
// --------------------------------------------------------------------------------

using System.Text;
using UnityEngine;

namespace UFramework.Editor.Preferences.Localization
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
            for (var i = 0; i < reader.sheets.Count; i++)
            {
                _builder.AppendLine($"\t{reader.sheets[i].name} = {i},");
            }

            _builder.AppendLine("}");

            // key
            _builder.AppendLine("language_key = {");

            for (var i = 0; i < reader.sheets.Count; i++)
            {
                _builder.AppendLine(reader.sheets[i].ToLuaKeyString());
            }

            _builder.AppendLine("}");

            // TODO
            // FilePathUtils.FileWriteAllText(FilePathUtils.Combine(Application.dataPath, "LuaScripts/language.lua"), m_builder.ToString());
        }
    }
}