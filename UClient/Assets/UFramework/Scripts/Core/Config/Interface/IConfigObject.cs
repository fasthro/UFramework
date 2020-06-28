/*
 * @Author: fasthro
 * @Date: 2020-05-31 21:50:41
 * @Description: 配置接口
 */

namespace UFramework.Config
{
    public interface IConfigObject
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        /// <value></value>
        string name { get; }

        /// <summary>
        /// Odin Readed
        /// </summary>
        object OdinReaded();

        /// <summary>
        /// 保存配置
        /// </summary>
        void Save();
    }
}
