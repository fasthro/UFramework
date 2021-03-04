// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-07-01 20:01:09
// * @Description:
// --------------------------------------------------------------------------------

using System.Text;

namespace UFramework.Editor.Preferences.Localization
{
    public class Excel2Index
    {
        static StringBuilder Builder = new StringBuilder();
        static StringBuilder FunBuilder = new StringBuilder();

        static string FuncTemplate = @"        public static string $model1$(int key)
        {
            return Localization.GetText($model2$, key);
        }";

        public Excel2Index(ExcelReader reader)
        {
            Builder.Clear();
            FunBuilder.Clear();

            // model
            Builder.AppendLine("// UFramework Automatic.");
            Builder.AppendLine("using UFramework.Core;");
            Builder.AppendLine("");
            Builder.AppendLine("namespace UFramework.Automatic {");
            Builder.AppendLine("\tpublic class Lang");
            Builder.AppendLine("\t{");

            for (var i = 0; i < reader.sheets.Count; i++)
            {
                var modelName = reader.sheets[i].name;
                Builder.AppendLine($"\t\tpublic static int {modelName} = {i};");

                var template = string.Copy(FuncTemplate);
                template = template.Replace("$model1$", "Get" + modelName.FirstCharToUpper());
                template = template.Replace("$model2$", modelName);
                FunBuilder.AppendLine(template);
                FunBuilder.AppendLine("");
            }

            // key
            Builder.AppendLine("");

            for (var i = 0; i < reader.sheets.Count; i++)
            {
                Builder.AppendLine(reader.sheets[i].ToCSharpKeyString());
            }

            Builder.AppendLine("");
            Builder.AppendLine(FunBuilder.ToString());
            Builder.AppendLine("\t}");
            Builder.AppendLine("}");

            IOPath.FileCreateText("Assets/Scripts/Automatic/Localization/Lang.cs", Builder.ToString());
        }
    }
}