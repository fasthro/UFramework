// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/02 16:34
// * @Description:
// --------------------------------------------------------------------------------

namespace UFramework.Consoles
{
    public interface IConsolePanelTab
    {
        bool isShowing { get; }
        bool initialized { get; }
        
        void Show();
        void Hide();
        void Refresh();
        void Update();
    }
}