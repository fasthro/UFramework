// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/08 14:32
// * @Description:
// --------------------------------------------------------------------------------

using UnityEngine;

namespace UFramework.Consoles
{
    public class FPSService : BaseConsoleService
    {
        /// <summary>
        /// FPS (value)
        /// </summary>
        public static float fpsValue { get; private set; }

        const float frequency = 0.5f; // 更新频率

        static float _accum;
        static int _frame;
        static float _accumTime;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nbDecimal">精度</param>
        /// <returns></returns>
        public string GetFPS(int nbDecimal = 0)
        {
            return fpsValue.ToString("f" + Mathf.Clamp(nbDecimal, 0, 10));
        }

        protected override void OnUpdate()
        {
            _accumTime += Time.deltaTime;
            _accum += Time.timeScale / Time.deltaTime;
            _frame++;
            if (_accumTime >= frequency)
            {
                fpsValue = _accum / (float) _frame;
                _accumTime = 0f;
                _accum = 0f;
                _frame = 0;
            }
        }
    }
}