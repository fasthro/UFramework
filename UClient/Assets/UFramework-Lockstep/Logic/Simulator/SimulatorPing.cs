// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/12 16:39
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Lockstep.Message;
using UnityEngine;

namespace Lockstep.Logic.Simulator
{
    public class SimulatorPing
    {
        const float COLLECT_INTERVAL_TIME = 1f;
        const float SEND_INTERVAL_TIME = 0.5f;

        /// <summary>
        /// Ping
        /// </summary>
        public int pingValue { get; private set; }

        /// <summary>
        /// Max Ping
        /// </summary>
        public long maxPingValue { get; private set; }

        /// <summary>
        /// Min Ping
        /// </summary>
        public long minPingValue { get; private set; }

        private float _collectTime;
        private float _sendTime;
        private List<long> _values = new List<long>();

        /// <summary>
        /// add ping value
        /// </summary>
        /// <param name="value"></param>
        public void AddValue(long value)
        {
            _values.Add(value);

            if (value > maxPingValue)
                maxPingValue = value;

            if (value < minPingValue)
                minPingValue = value;
        }

        public void Update()
        {
            // collect
            if (Time.realtimeSinceStartup - _collectTime > COLLECT_INTERVAL_TIME)
            {
                _collectTime = Time.realtimeSinceStartup;
                pingValue = (int) _values.Sum() / LSMath.Max(_values.Count, 1);
                _values.Clear();
            }

            // send
            if (Time.realtimeSinceStartup - _sendTime > SEND_INTERVAL_TIME)
            {
                _sendTime = Time.realtimeSinceStartup;
                NetworkProxy.Send(NetworkCmd.PING, new Ping_C2S() {SendTimestamp = LSTime.realtimeSinceStartupMS});
            }
        }
    }
}