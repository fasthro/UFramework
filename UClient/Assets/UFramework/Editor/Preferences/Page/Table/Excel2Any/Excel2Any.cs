// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-06-29 11:26:04
// * @Description:
// --------------------------------------------------------------------------------

namespace UFramework.Editor.Preferences.Table
{
    public abstract class Excel2Any
    {
        protected ExcelReader _reader;

        public Excel2Any(ExcelReader reader)
        {
            this._reader = reader;
        }
    }
}