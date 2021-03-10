// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/10 10:46
// * @Description:
// --------------------------------------------------------------------------------

namespace UFramework.Consoles
{
    public class HistoryCommand
    {
        public Command command { get; private set; }
        public string content { get; private set; }

        public HistoryCommand(Command command, string content)
        {
            this.command = command;
            this.content = content;
        }
    }
}