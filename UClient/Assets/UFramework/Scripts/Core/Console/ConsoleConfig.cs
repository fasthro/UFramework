// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/03 16:32
// * @Description:
// --------------------------------------------------------------------------------

using UFramework.Core;

namespace UFramework.Consoles
{
    public class ConsoleConfig : ISerializable
    {
        public SerializableAssigned assigned => SerializableAssigned.Resources;
        
        #region trigger

        public TriggerAlignment triggerAlignment = TriggerAlignment.TopLeft;
        public int triggerHScreenAspect = 10;
        public int triggerVScreenAspect = 10;
        public int triggerTime = 2;

        #endregion

        #region log

        public bool collapseLogEntries = true;
        public int maxLogEntries = 2000;
        
        public bool enabledWriteFile = true;
        public int writeIntervalTime = 1;
        
        #endregion

        #region profiler

        public bool enabledMemory = true;
        public int memoryIntervalTime = 1;
        
        #endregion

        public void Serialize()
        {
            Serializer<ConsoleConfig>.Serialize(this);
        }
    }
}