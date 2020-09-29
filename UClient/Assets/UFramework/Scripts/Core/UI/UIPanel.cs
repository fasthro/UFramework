/*
 * @Author: fasthro
 * @Date: 2020-09-29 11:36:31
 * @Description: panel base
 */
namespace UFramework.UI
{
    public abstract class UIPanel
    {
        /// <summary>
        /// 层
        /// </summary>
        public Layer layer { get; protected set; }

        /// <summary>
        /// 更新Panel排序
        /// </summary>
        /// <param name="order"></param>
        public abstract void UpdateSortOrder(int order);
    }
}