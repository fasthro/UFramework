/*
 * @Author: fasthro
 * @Date: 2020-06-29 08:38:48
 * @Description: 页面接口
 */
namespace UFramework.FrameworkWindow
{
    public interface IPage
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        /// <value></value>
        string menuName { get; }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        object GetInstance();

        /// <summary>
        /// 渲染之前
        /// </summary>
        void OnRenderBefore();

        /// <summary>
        /// 是否为
        /// </summary>
        /// <returns></returns>
        bool IsPage(string name);
    }
}