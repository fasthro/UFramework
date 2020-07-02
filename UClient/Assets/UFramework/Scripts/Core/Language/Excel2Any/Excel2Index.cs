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
        static StringBuilder builder = new StringBuilder();
        static StringBuilder funBuilder = new StringBuilder();

        static string funcTemplate = @"    public static string $model1$(int key)
    {
        return sys.Get($model2$, key);
    }";

        public Excel2Index(ExcelReader reader)
        {
            builder.Clear();
            funBuilder.Clear();

            // model
            builder.AppendLine("// UFramework Auto Generate.");
            builder.AppendLine("using sys = UFramework.i18n.Language;");
            builder.AppendLine("");
            builder.AppendLine("public static class Language");
            builder.AppendLine("{");

            for (int i = 0; i < reader.sheets.Count; i++)
            {
                var modelName = reader.sheets[i].name;
                builder.AppendLine(string.Format("\tpublic static int {0} = {1};", modelName, i));

                var template = string.Copy(funcTemplate);
                template = template.Replace("$model1$", "Get" + modelName.FirstCharToUpper());
                template = template.Replace("$model2$", modelName);
                funBuilder.AppendLine(template);
                funBuilder.AppendLine("");
            }

            // key
            builder.AppendLine("");

            for (int i = 0; i < reader.sheets.Count; i++)
            {
                builder.AppendLine(reader.sheets[i].ToCSharpKeyString());
            }
            
            builder.AppendLine("");
            builder.AppendLine(funBuilder.ToString());
            builder.AppendLine("}");

            IOPath.FileCreateText(App.LanguageIndexDirectory + "/Language.cs", builder.ToString());
        }
    }
}