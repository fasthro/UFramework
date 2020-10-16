/*
 * @Author: fasthro
 * @Date: 2020-10-15 12:26:08
 * @Description: 
 */
using UFramework.Config;

namespace UFramework.Editor.VersionControl
{
    public class VersionControl_BuildConfig : IConfigObject
    {
        public string name { get { return "VersionControl_BuildConfig"; } }
        public FileAddress address { get { return FileAddress.Editor; } }

        public void Save()
        {
            UConfig.Write<VersionControl_BuildConfig>(this);
        }
    }
}