/*
 * @Author: fasthro
 * @Date: 2020-01-04 17:52:23
 * @Description: excel 2 text data
 */
namespace UFramework.Language
{
    public class Excel2Text
    {
        public Excel2Text(ExcelReader reader)
        {
            var dataPath = IOPath.PathCombine(App.AssetsDirectory, "Language", "Data");
            IOPath.DirectoryClear(dataPath);

            for (int i = 0; i < reader.sheets.Count; i++)
            {
                var sheet = reader.sheets[i];
                for (int l = 0; l < reader.options.languages.Count; l++)
                {
                    var language = reader.options.languages[l];
                    IOPath.FileCreateText(IOPath.PathCombine(dataPath, language.ToString(), i.ToString() + ".txt"), sheet.ToValueString(l + 1));
                }
            }
        }
    }
}