// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/02 16:34
// * @Description:
// --------------------------------------------------------------------------------

namespace UFramework.Consoles
{
    public interface IConsolePanelTab
    {
        bool initialized { get; }
        
        void DoShow();
        void DoHide();
        void DoRefresh();
        void DoUpdate();
    }
}