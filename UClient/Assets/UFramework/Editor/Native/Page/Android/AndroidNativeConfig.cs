/*
 * @Author: fasthro
 * @Date: 2020-08-29 12:19:07
 * @Description: Android Native Config
 */
using UFramework.Config;

namespace UFramework.Editor.Native
{
    public class AndroidNativeConfig : IConfigObject
    {
        public string name { get { return "AndroidNativeConfig"; } }

        /// <summary>
        /// Modules
        /// </summary>
        public string[] modules;

        public void Save()
        {
            UConfig.Write<AndroidNativeConfig>(this);
        }
    }
}