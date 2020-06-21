/*
 * @Author: fasthro
 * @Date: 2020-06-21 10:26:01
 * @Description: timer info data
 */

using UFramework.Messenger;
using LuaInterface;

namespace UFramework.Timer
{
    public class TimerInfo
    {
        public string name;
        public float expire;
        public float tick;
        public float interval;
        public object param;
        public LuaTable luaself;
        public LuaFunction luaFunc;
        public UCallback<object> callback;
    }
}