// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/02 14:28
// * @Description:
// --------------------------------------------------------------------------------

namespace UFramework.Consoles
{
    public class BaseConsoleService
    {
        public void Initialize()
        {
            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
        }

        public void Update()
        {
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
        }
    }
}