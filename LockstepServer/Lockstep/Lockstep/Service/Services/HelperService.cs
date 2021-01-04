/*
 * @Author: fasthro
 * @Date: 2020/12/31 12:28:54
 * @Description:
 */

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