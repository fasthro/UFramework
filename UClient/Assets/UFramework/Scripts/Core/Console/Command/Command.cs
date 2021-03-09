// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/09 14:46
// * @Description:
// --------------------------------------------------------------------------------

using UnityEngine;

namespace UFramework.Consoles
{
    public abstract class Command
    {
        /// <summary>
        /// 命令名称
        /// </summary>
        public string name { get; private set; }
        
        /// <summary>
        /// 命令描述
        /// </summary>
        public string description { get; private set; }
        
        /// <summary>
        /// 命令默认参数语句
        /// </summary>
        public string paramStatement { get; private set; }

        public Command(string name, string description = "", string paramStatement = "")
        {
            this.name = name;
            this.description = description;
            this.paramStatement = paramStatement;
        }

        public virtual void Execute(string line)
        {
        }

        public override string ToString()
        {
            return name;
        }

        protected void LogError(object message)
        {
            Debug.LogError("[Console-Command] " + message.ToString());
        }
    }
}