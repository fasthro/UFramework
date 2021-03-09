// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/08 17:27
// * @Description:
// --------------------------------------------------------------------------------

using System;
using System.Collections;
using UFramework.Core;
using UnityEngine;
using UnityEngine.Profiling;

namespace UFramework.Consoles
{
    public class MemoryService : BaseConsoleService
    {
        public long maxMonoMemory { get; private set; }
        public long monoMemory { get; private set; }

        public long maxMemory { get; private set; }
        public long memory { get; private set; }

        private bool _isMonoSupported;

        private bool _enabledMemory;
        private float _memoryIntervalTime;

        private float _memoryIntervalTimeTemp;

        protected override void OnInitialized()
        {
            var consoleConfig = Serializer<ConsoleConfig>.Instance;

            _enabledMemory = consoleConfig.enabledMemory;
            _memoryIntervalTime = (float)consoleConfig.memoryIntervalTime;
            
            if (!_enabledMemory)
                return;

            _isMonoSupported = Profiler.GetMonoUsedSizeLong() > 0;
        }

        protected override void OnUpdate()
        {
            if (!_enabledMemory)
                return;

            if (Time.realtimeSinceStartup - _memoryIntervalTimeTemp> _memoryIntervalTime)
            {
                maxMemory = Profiler.GetTotalReservedMemoryLong();
                memory = Profiler.GetTotalAllocatedMemoryLong();

                maxMonoMemory = _isMonoSupported ? Profiler.GetMonoHeapSizeLong() : System.GC.GetTotalMemory(false);
                monoMemory = Profiler.GetMonoUsedSizeLong();

                _memoryIntervalTimeTemp = Time.realtimeSinceStartup;
            }
        }

        public void GC()
        {
            System.GC.Collect();
        }

        public void Clean()
        {
            Core.Coroutine.Allocate(CleanUp());
        }

        private IEnumerator CleanUp()
        {
            System.GC.Collect();
            yield return Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
    }
}