// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/02 15:43
// * @Description:
// --------------------------------------------------------------------------------

using FairyGUI;
using UFramework.UI;

namespace UFramework.Consoles
{
    public class ConsolePanel : FiaryPanel
    {
        #region component

        private GComboBox _functionComboBox;
        private GButton _closeButton;

        #endregion

        #region tabs

        private ConsolePanel_LogTab _logTab;

        #endregion

        public static ConsolePanel Create()
        {
            var panel = new ConsolePanel {isResourceLoad = true};
            panel.AddPackage("RP_Console");
            return panel;
        }

        public ConsolePanel() : base("Console", "RP_Console", Layer.Console)
        {
            _logTab = new ConsolePanel_LogTab(this);
        }

        protected override void OnShow()
        {
            base.OnShow();

            _functionComboBox = view.GetChild("_function").asComboBox;
            _functionComboBox.onChanged.Set(OnFunctionComboBoxChanged);

            _closeButton = view.GetChild("_close").asButton;
            _closeButton.onClick.Set(OnCloseClick);

            _logTab.DoShow();
        }

        private void OnFunctionComboBoxChanged()
        {
            
        }

        private void OnCloseClick()
        {
            Hide();
        }

        public void DoUpdate()
        {
            if (!isShowed)
                return;
            
            _logTab?.DoUpdate();
        }
    }
}