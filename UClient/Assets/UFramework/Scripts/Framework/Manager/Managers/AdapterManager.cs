// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/24 16:58
// * @Description:
// --------------------------------------------------------------------------------

using FairyGUI;

namespace UFramework
{
    public class AdapterManager : BaseManager
    {
        #region fairy

        /// <summary>
        /// Fairy 查询面板对象
        /// </summary>
        /// <param name="panelName">面板名称</param>
        /// <param name="path">组件路径("/" 分割多级路径)，如果获取最上级View参数为空字符串</param>
        /// <returns></returns>
        public GObject FairyQueryObject(string panelName, string path)
        {
            if (_luaManager.luaEngine == null)
                return null;

            return _luaManager.luaEngine.Invoke<string, string, GObject>("fairyQueryObject", panelName, path);
        }

        /// <summary>
        /// Fairy 查询面板组件
        /// </summary>
        /// <param name="panelName">面板名称</param>
        /// <param name="path">组件路径("/" 分割多级路径)，如果获取最上级View参数为空字符串</param>
        /// <returns></returns>
        public GComponent FairyQueryComponent(string panelName, string path)
        {
            return FairyQueryObject(panelName, path)?.asCom;
        }

        /// <summary>
        /// Fairy 查询面板组件动画
        /// </summary>
        /// <param name="panelName">面板名称</param>
        /// <param name="path">组件路径("/" 分割多级路径)，如果获取最上级View参数为空字符串</param>
        /// <param name="name">动画名称</param>
        /// <returns></returns>
        public Transition FairyQueryTransition(string panelName, string path, string name)
        {
            return FairyQueryComponent(panelName, path)?.GetTransition(name);
        }

        /// <summary>
        /// Fairy 查询面板组件控制器
        /// </summary>
        /// <param name="panelName">面板名称</param>
        /// <param name="path">组件路径("/" 分割多级路径)，如果获取最上级View参数为空字符串</param>
        /// <param name="name">控制器名称</param>
        /// <returns></returns>
        public Controller FairyQueryController(string panelName, string path, string name)
        {
            return FairyQueryComponent(panelName, path)?.GetController(name);
        }

        #endregion
    }
}