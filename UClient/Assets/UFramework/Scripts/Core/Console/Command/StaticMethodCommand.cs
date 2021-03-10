// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/09 14:46
// * @Description:
// --------------------------------------------------------------------------------

using System.Reflection;

namespace UFramework.Consoles
{
    public class StaticMethodCommand : MethodCommand
    {
        public StaticMethodCommand(string name, string description, string paramStatement, MethodInfo methodInfo)
            : base(name, description, paramStatement, methodInfo, null)
        {
        }
    }
}