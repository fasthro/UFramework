// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021-01-06 16:10:08
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace Lockstep.Logic.Simulator
{
    public class FrameBuffer
    {
        /// <summary>
        /// 缓冲容量
        /// </summary>
        /// <value></value>
        public int capacity { get; private set; }

        /// <summary>
        /// 服务器当前帧
        /// </summary>
        /// <value></value>
        public int sTick { get; private set; }

        /// <summary>
        /// 客户端当前帧
        /// </summary>
        /// <value></value>
        public int cTick { get; private set; }

        /// <summary>
        /// 当前执行帧
        /// </summary>
        /// <value></value>
        public int rTick { get; private set; }

        #region private frame

        private FrameData[] _sFrames;
        private FrameData[] _cFrames;
        private RollbackInfo _rollbackInfo;

        #endregion


        public FrameBuffer(int capacity = 2000) : base()
        {
            this.capacity = capacity;
            _sFrames = new FrameData[capacity];
            _cFrames = new FrameData[capacity];
            _rollbackInfo = new RollbackInfo();
        }

        public void Rollback(int tick)
        {
            cTick = tick;
            rTick = tick;
        }

        public void PushCFrame(FrameData frame)
        {
            var index = FrameIndex(frame);
            _cFrames[index] = frame;
            cTick = frame.tick > cTick ? frame.tick : cTick;
        }

        public RollbackInfo PushSFrame(FrameData frame)
        {
            var index = FrameIndex(frame);
            _sFrames[index] = frame;
            sTick = frame.tick > sTick ? frame.tick : sTick;

            if (!FrameData.CheckEquals(frame, GetCFrame(sTick)))
            {
                _rollbackInfo.tick = sTick - 1;
                return _rollbackInfo;
            }

            return null;
        }

        public FrameData GetFrame(int tick)
        {
            rTick = tick;
            var frame = GetSFrame(tick);
            return frame ?? GetCFrame(tick);
        }

        public FrameData GetSFrame(int tick)
        {
            return tick > sTick ? null : _sFrames[TickIndex(tick)];
        }

        public FrameData GetCFrame(int tick)
        {
            return tick > cTick ? null : _cFrames[TickIndex(tick)];
        }

        public void Reset()
        {
            _cFrames = new FrameData[capacity];
            _sFrames = new FrameData[capacity];
            cTick = sTick = 0;
        }

        private int FrameIndex(FrameData frame)
        {
            return TickIndex(frame.tick);
        }

        private int TickIndex(int tick)
        {
            return tick % capacity;
        }
    }
}