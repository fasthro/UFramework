/*
 * @Author: fasthro
 * @Date: 2020-06-29 08:38:48
 * @Description: IPage
 */
namespace UFramework.Editor.Preferences
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
        /// 画功能按钮
        /// </summary>
        void OnDrawFunctoinButton();

        /// <summary>
        /// 应用
        /// </summary>
        void OnApply();
    }
}