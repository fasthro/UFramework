// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/08 17:27
// * @Description:
// --------------------------------------------------------------------------------

namespace UFramework.Consoles
{
    public class MemoryService : BaseConsoleService
    {
        protected override void OnUpdate()
        {
            // max = Profiler.GetTotalReservedMemoryLong();
            // current = Profiler.GetTotalAllocatedMemoryLong();
            // var maxMb = (max >> 10);
            // maxMb /= 1024; // On new line to fix il2cpp
            //
            // var currentMb = (current >> 10);
            // currentMb /= 1024;
            
            // GC.Collect();
            
            // _isSupported = Profiler.GetMonoUsedSizeLong() > 0;
            // max = _isSupported ? Profiler.GetMonoHeapSizeLong() : GC.GetTotalMemory(false);
            // current = Profiler.GetMonoUsedSizeLong();
            
            // GC.Collect();
            // yield return Resources.UnloadUnusedAssets();
            // GC.Collect();

        }
    }
}