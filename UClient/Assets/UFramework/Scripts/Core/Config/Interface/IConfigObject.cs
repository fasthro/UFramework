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
        /// 配置文件路径
        /// </summary>
        /// <value></value>
        FileAddress address { get; }

        /// <summary>
        /// 保存配置
        /// </summary>
        void Save();
    }
}
