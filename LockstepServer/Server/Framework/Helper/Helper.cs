// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;

namespace UFramework
{
    public static class Helper
    {
        public static int Random(int min, int max)
        {
            var ran = new System.Random();
            return ran.Next(min, max);
        }

        public static long NewGuidId()
        {
            var buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        public static async Task Waitforms(int ms)
        {
            await Task.Run(() => { Thread.Sleep(ms); });
        }
    }
}