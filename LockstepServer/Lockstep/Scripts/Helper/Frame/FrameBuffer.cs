/*
 * @Author: fasthro
 * @Date: 2021-01-06 16:10:08
 * @Description: 
 */

namespace Lockstep
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

        #region  private

        // 服务器帧
        private FrameData[] _sFrames;

        // 客户端帧
        private FrameData[] _cFrames;

        #endregion

        public FrameBuffer(int capacity = 2000)
        {
            this.capacity = capacity;
            _sFrames = new FrameData[capacity];
            _cFrames = new FrameData[capacity];
        }

        public void PushCFrame(FrameData frame)
        {
            var index = FrameIndex(frame);
            _cFrames[index] = frame;
            cTick = frame.tick > cTick ? frame.tick : cTick;
        }

        public void PushSFrame(FrameData frame)
        {
            var index = FrameIndex(frame);
            _sFrames[index] = frame;
            sTick = frame.tick > sTick ? frame.tick : sTick;
        }

        public FrameData GetFrame(int tick)
        {
            var frame = GetSFrame(tick);
            return frame == null ? GetCFrame(tick) : frame;
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