// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/09 14:46
// * @Description:
// --------------------------------------------------------------------------------

using System;

namespace UFramework.Consoles
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class MethodCommandAttribute : Attribute
    {
        /// <summary>
        /// 命令名称
        /// </summary>
        public readonly string name;

        /// <summary>
        /// 命令描述
        /// </summary>
        public readonly string description;

        /// <summary>
        /// 命令默认参数语句
        /// </summary>
        public readonly string paramStatement;

        public MethodCommandAttribute(string name, string description = "", string paramStatement = "")
        {
            this.name = name;
            this.description = description;
            this.paramStatement = paramStatement;
        }
    }
}