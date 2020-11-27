/*
 * @Author: fasthro
 * @Date: 2020-11-30 18:22:50
 * @Description: pbc
 */

using System.Text;

namespace UFramework.Network
{
    public class SocketPackPBC : SocketPackLinearBinary
    {
        public override ProtocalType protocal { get { return ProtocalType.PBC; } }
    }
}