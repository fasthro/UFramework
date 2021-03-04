﻿// --------------------------------------------------------------------------------
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

        #region adv

        public bool collapseLogEntries = true;
        public bool saveLogFile = true;
        public int maxLogEntries = 2000;
        
        #endregion

        public void Serialize()
        {
            Serializer<ConsoleConfig>.Serialize(this);
        }
    }
}