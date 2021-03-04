// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-07-01 20:01:09
// * @Description:
// --------------------------------------------------------------------------------

namespace UFramework.Editor.Preferences.Localization
{
    public class Excel2Text
    {
        public Excel2Text(ExcelReader reader)
        {
            var dataPath = IOPath.PathCombine(UApplication.AssetsDirectory, "Language", "Data");
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