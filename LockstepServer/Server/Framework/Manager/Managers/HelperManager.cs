// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

namespace UFramework
{
    public class HelperManager : BaseManager, IHelperManager
    {
        public override void Initialize()
        {
            LogHelper.Initialize();
        }
    }
}