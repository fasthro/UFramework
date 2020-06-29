/*
 * @Author: fasthro
 * @Date: 2020-06-29 17:00:30
 * @Description: AssetBundle
 */
using System.Collections.Generic;

namespace UFramework.FrameworkWindow
{
    public class AssetBundlePage : IPage
    {
        public string menuName { get { return "AssetBundle"; } }

        public List<string> resPaths = new List<string>();

        public object GetInstance()
        {
            return this;
        }

        public bool IsPage(string name)
        {
            return menuName.Equals(name);
        }

        public void OnRenderBefore()
        {

        }
    }
}