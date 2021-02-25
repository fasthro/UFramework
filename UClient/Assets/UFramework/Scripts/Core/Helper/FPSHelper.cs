// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/25 16:27
// * @Description:
// --------------------------------------------------------------------------------

using UnityEngine;

namespace UFramework
{
    public static class FPSHelper
    {
        /// <summary>
        /// FPS (string)
        /// </summary>
        public static string fps { get; private set; }

        /// <summary>
        /// FPS (value)
        /// </summary>
        public static float fpsValue { get; private set; }
        
        const float frequency = 0.5f; // 更新频率
        const int nbDecimal = 0; // fps精度

        static float _accum;
        static int _frame;
        static float _accumTime;

        public static void Update()
        {
            _accumTime += Time.deltaTime;
            _accum += Time.timeScale / Time.deltaTime;
            _frame++;
            if (_accumTime >= frequency)
            {
                fpsValue = _accum / (float) _frame;
                fps = fpsValue.ToString("f" + Mathf.Clamp(nbDecimal, 0, 10));
                _accumTime = 0f;
                _accum = 0f;
                _frame = 0;
            }
        }
    }
}