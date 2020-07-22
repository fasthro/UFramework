/*
 * @Author: fasthro
 * @Date: 2020-07-23 00:54:22
 * @Description: window page interface
 */
namespace UFramework.Editor
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
    }
}