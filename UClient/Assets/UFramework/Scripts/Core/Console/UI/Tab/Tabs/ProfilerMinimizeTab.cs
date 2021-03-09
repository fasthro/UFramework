// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/08 14:52
// * @Description:
// --------------------------------------------------------------------------------

using FairyGUI;
using UFramework.Core;

namespace UFramework.Consoles
{
    public class ProfilerMinimizeTab : BaseConsoleTab
    {
        private static FPSService fpsService;
        private static MemoryService memoryService;

        #region component

        private GTextField _fps;

        private GProgressBar _monoBar;
        private GRichTextField _monoText;

        private GProgressBar _memoryBar;
        private GRichTextField _memoryText;

        #endregion

        public ProfilerMinimizeTab(ConsolePanel consolePanel) : base(consolePanel)
        {
            fpsService = Console.Instance.GetService<FPSService>();
            memoryService = Console.Instance.GetService<MemoryService>();
        }

        protected override void OnInitialize()
        {
            _view = _consolePanel.view.GetChild("_profilerMinTab").asCom;

            _fps = _view.GetChild("_fps").asTextField;

            _monoBar = _view.GetChild("_mono").asProgress;
            _monoText = _monoBar.GetChild("_text").asRichTextField;

            _memoryBar = _view.GetChild("_memory").asProgress;
            _memoryText = _memoryBar.GetChild("_text").asRichTextField;
        }

        protected override void OnUpdate()
        {
            _fps.text = $"FPS:{fpsService.GetFPS(2).ToString()}";

            _memoryBar.max = memoryService.maxMemory;
            _memoryBar.value = memoryService.memory;
            _memoryText.text = $"{Utils.FormatBytes(memoryService.maxMemory)}/{Utils.FormatBytes(memoryService.memory)}";

            _monoBar.max = memoryService.maxMonoMemory;
            _monoBar.value = memoryService.monoMemory;
            _monoText.text = $"{Utils.FormatBytes(memoryService.monoMemory)}/{Utils.FormatBytes(memoryService.maxMonoMemory)}";
        }
    }
}