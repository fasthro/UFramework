// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/02 16:34
// * @Description:
// --------------------------------------------------------------------------------

namespace UFramework.Consoles
{
    public interface IConsolePanelTab
    {
        void DoShow();
        void DoHide();
        void DoRefresh();
        void DoUpdate();
    }
}