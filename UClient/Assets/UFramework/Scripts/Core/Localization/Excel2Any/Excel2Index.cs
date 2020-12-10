/*
 * @Author: fasthro
 * @Date: 2020-01-04 17:52:23
 * @Description: excel 2 c# index
 */
using System.Text;

namespace UFramework.Language
{
    public class Excel2Index
    {
        static StringBuilder Builder = new StringBuilder();
        static StringBuilder FunBuilder = new StringBuilder();

        static string FuncTemplate = @"        public static string $model1$(int key)
        {
            return sys.Get($model2$, key);
        }";

        public Excel2Index(ExcelReader reader)
        {
            Builder.Clear();
            FunBuilder.Clear();

            // model
            Builder.AppendLine("// UFramework Automatic.");
            Builder.AppendLine("using sys = UFramework.Localization.Language;");
            Builder.AppendLine("");
            Builder.AppendLine("namespace UFramework.Automatic {");
            Builder.AppendLine("\tpublic class Language");
            Builder.AppendLine("\t{");

            for (int i = 0; i < reader.sheets.Count; i++)
            {
                var modelName = reader.sheets[i].name;
                Builder.AppendLine(string.Format("\t\tpublic static int {0} = {1};", modelName, i));

                var template = string.Copy(FuncTemplate);
                template = template.Replace("$model1$", "Get" + modelName.FirstCharToUpper());
                template = template.Replace("$model2$", modelName);
                FunBuilder.AppendLine(template);
                FunBuilder.AppendLine("");
            }

            // key
            Builder.AppendLine("");

            for (int i = 0; i < reader.sheets.Count; i++)
            {
                Builder.AppendLine(reader.sheets[i].ToCSharpKeyString());
            }

            Builder.AppendLine("");
            Builder.AppendLine(FunBuilder.ToString());
            Builder.AppendLine("\t}");
            Builder.AppendLine("}");

            IOPath.FileCreateText("Assets/Scripts/Automatic/Localization/Language.cs", Builder.ToString());
        }
    }
}