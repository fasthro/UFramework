using log4net;
using log4net.Config;
using log4net.Repository;

/*
 * @Author: fasthro
 * @Date: 2020/12/18 14:44:00
 * @Description:
 */

using System.IO;

namespace LockstepServer
{
    public class AppServer : BaseBehaviour
    {
        private UdpSocketServer _udpSocketServer;
        public bool IsRunning => _udpSocketServer.isRunning;

        /// <summary>
        /// 启动服务器
        /// </summary>
        /// <param name="port"></param>
        public void StartServer(int port)
        {
            _udpSocketServer.StartServer(port);

            LogHelper.Info("AppServer Launched!");
        }

        /// <summary>
        /// 停止服务器
        /// </summary>
        public void StopServer()
        {
            _udpSocketServer.StopServer();
        }

        protected override void OnInitialize()
        {
            LogHelper.Initialize();

            // 启动服务
            Service.Instance.Default();

            //创建服务器网络监听
            var netListener = Service.Instance.GetManager<NetManager>() as IServerListener;
            _udpSocketServer = new UdpSocketServer(netListener);

            LogHelper.Info("AppServer Initialized!");
        }

        protected override void OnUpdate(float deltaTime)
        {
            _udpSocketServer.Update();
        }
    }
}