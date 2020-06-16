/*
 * @Author: fasthro
 * @Date: 2020-05-31 22:14:16
 * @Description: Config 配置数据
 */
using System.Collections.Generic;

namespace UFramework.Config
{
    public class BaseConfig : IConfigObject
    {
        public string name { get; }

        public Dictionary<string, FileAddress> addressDictionary = new Dictionary<string, FileAddress>();
    }
}