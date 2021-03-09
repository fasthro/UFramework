// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/08 14:52
// * @Description:
// --------------------------------------------------------------------------------

using FairyGUI;
using UFramework.Core;

namespace UFramework.Consoles
{
    public class ProfilerMaximizeTab : BaseConsoleTab
    {
        private const string COLOR_FMT = "{0}<font color=#BCBCBC>{1}</font>";
        private static FPSService fpsService;
        private static MemoryService memoryService;

        #region component

        private GTextField _fps;

        private GButton _minimizeBtn;
        private GButton _gcBtn;
        private GButton _cleanBtn;

        private GProgressBar _monoBar;
        private GRichTextField _maxMonoText;
        private GRichTextField _monoText;

        private GProgressBar _memoryBar;
        private GRichTextField _maxMemoryText;
        private GRichTextField _memoryText;

        #endregion

        public ProfilerMaximizeTab(ConsolePanel consolePanel) : base(consolePanel)
        {
            fpsService = Console.Instance.GetService<FPSService>();
            memoryService = Console.Instance.GetService<MemoryService>();
        }

        protected override void OnInitialize()
        {
            _view = _consolePanel.view.GetChild("_profilerMaxTab").asCom;

            _fps = _view.GetChild("_fps").asTextField;

            _minimizeBtn = _view.GetChild("_minimize").asButton;
            _minimizeBtn.onClick.Set(OnMinimizeClick);

            var monoCom = _view.GetChild("_mono").asCom;
            monoCom.GetChild("_title").text = "Mono Memory Usage";

            _monoBar = monoCom.GetChild("_bar").asProgress;
            _maxMonoText = _monoBar.GetChild("_total").asRichTextField;
            _monoText = _monoBar.GetChild("_cur").asRichTextField;

            _gcBtn = monoCom.GetChild("_btn").asButton;
            _gcBtn.title = "GC";
            _gcBtn.onClick.Set(OnGCClick);

            var memoryCom = _view.GetChild("_memory").asCom;
            memoryCom.GetChild("_title").text = "Memory Usage";

            _memoryBar = memoryCom.GetChild("_bar").asProgress;
            _maxMemoryText = _memoryBar.GetChild("_total").asRichTextField;
            _memoryText = _memoryBar.GetChild("_cur").asRichTextField;

            _cleanBtn = memoryCom.GetChild("_btn").asButton;
            _cleanBtn.title = "Clean";
            _cleanBtn.onClick.Set(OnCleanClick);
        }

        protected override void OnUpdate()
        {
            _fps.text = $"FPS:{fpsService.GetFPS(2).ToString()}";

            _memoryBar.max = memoryService.maxMemory;
            _memoryBar.value = memoryService.memory;
            _maxMemoryText.text = Utils.FormatBytes(memoryService.maxMemory, COLOR_FMT);
            _memoryText.text = Utils.FormatBytes(memoryService.memory, COLOR_FMT);

            _monoBar.max = memoryService.maxMonoMemory;
            _monoBar.value = memoryService.monoMemory;
            _maxMonoText.text = Utils.FormatBytes(memoryService.maxMonoMemory, COLOR_FMT);
            _monoText.text = Utils.FormatBytes(memoryService.monoMemory, COLOR_FMT);
        }

        private void OnMinimizeClick()
        {
            _consolePanel.ShowTab(ConsolePanelTab.ProfilerMinimize);
        }

        private void OnGCClick()
        {
            memoryService.GC();
        }

        private void OnCleanClick()
        {
            memoryService.Clean();
        }
    }
}