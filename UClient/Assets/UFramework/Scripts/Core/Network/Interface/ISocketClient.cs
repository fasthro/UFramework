/*
 * @Author: fasthro
 * @Date: 2020-12-16 17:24:09
 * @Description: 
 */
namespace UFramework.Network
{
    public interface ISocketClient
    {
        bool isConnected { get; }
        
        void Update();
        void Connect(string ip, int port);
        void Send(SocketPack pack);
        void Disconnect();
        void Dispose();
    }
}