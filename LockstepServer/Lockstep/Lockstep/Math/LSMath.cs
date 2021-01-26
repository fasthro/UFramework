namespace Lockstep
{
    public static class LSMath
    {
        public static readonly Fix64 Rad2Deg = (Fix64) 180 / Fix64.Pi;
        public static readonly Fix64 Deg2Rad = Fix64.Pi / (Fix64) 180;

        public static readonly Fix64 Value180 = (Fix64) 180;
        public static readonly Fix64 Value360 = (Fix64) 360;

        #region 旋转

        /// <summary>
        /// Axis 转成角度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Fix64 ToDeg(LSVector2 value)
        {
            var ccwDeg = Fix64.Atan2(value.y, value.x) * LSMath.Rad2Deg;
            var deg = (Fix64) 90 - ccwDeg;
            return LSMath.AbsDeg(deg);
        }

        /// <summary>
        /// 旋转绝对值
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        public static Fix64 AbsDeg(Fix64 deg)
        {
            return deg % ((Fix64) 360);
        }

        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="targetAxis">目标轴向</param>
        /// <param name="currentAxis">当前轴向</param>
        /// <param name="deg">当前角度</param>
        /// <param name="turnValue">旋转角度</param>
        /// <param name="isLessDeg"></param>
        /// <returns></returns>
        public static Fix64 TurnToward(LSVector2 targetAxis, LSVector2 currentAxis, Fix64 deg, Fix64 turnValue, out bool isLessDeg)
        {
            var toTarget = (targetAxis - currentAxis).normalized;
            var toDeg = LSMath.ToDeg(toTarget);
            return TurnToward(toDeg, deg, turnValue, out isLessDeg);
        }

        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="target">目标角度</param>
        /// <param name="deg">当前角度</param>
        /// <param name="turnValue">旋转角度</param>
        /// <param name="isLessDeg"></param>
        /// <returns></returns>
        public static Fix64 TurnToward(Fix64 target, Fix64 deg, Fix64 turnValue, out bool isLessDeg)
        {
            var curDeg = LSMath.AbsDeg(deg);
            var diff = target - curDeg;
            var absDiff = Fix64.Abs(diff);
            isLessDeg = absDiff < turnValue;
            if (isLessDeg)
            {
                return target;
            }
            else
            {
                if (absDiff > Value180)
                {
                    if (diff > Fix64.Zero)
                        diff -= Value360;
                    else
                        diff += Value360;
                }

                return curDeg + turnValue * Fix64.Sin(diff);
            }
        }

        #endregion
    }
}