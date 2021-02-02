// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/29 16:31:41
// * @Description:
// --------------------------------------------------------------------------------

namespace Lockstep
{
    public class HelperService : BaseService, IHelperService
    {
        public override void Initialize()
        {
            LSTime.Initialize();
        }

        public override void Update()
        {
            LSTime.Update();
        }
    }
}