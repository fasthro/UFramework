// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/25 16:24
// * @Description:
// --------------------------------------------------------------------------------

namespace UFramework
{
    [MonoSingletonPath("UFramework/Helper")]
    public class Helper : MonoSingleton<Helper>
    {
        protected override void OnSingletonUpdate(float deltaTime)
        {
            FPSHelper.Update();
        }
    }
}