// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/08 15:04
// * @Description:
// --------------------------------------------------------------------------------

namespace UFramework.Consoles
{
    public class CommandTab :BaseConsoleTab, IConsolePanelTab
    {
        public CommandTab(ConsolePanel consolePanel) : base(consolePanel)
        {
        }

        protected override void OnInitialize()
        {
            
        }

        public void DoShow()
        {
            Initialize();
        }

        public void DoHide()
        {
            
        }

        public void DoRefresh()
        {
            
        }

        public void DoUpdate()
        {
            
        }
        
    }
}