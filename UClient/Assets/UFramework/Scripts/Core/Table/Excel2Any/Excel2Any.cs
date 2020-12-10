/*
 * @Author: fasthro
 * @Date: 2019-12-17 20:45:12
 * @Description: excel 2 any
 */
namespace UFramework.Table
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