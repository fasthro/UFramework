using log4net;
using log4net.Config;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LockstepServer
{
    public class AppServer : BaseBehaviour
    {
        public static ILoggerRepository repository { get; set; }
        private static ILog logger = null;

        public bool IsRunning => _server.isRunning;

        private ServerListener _server;

        protected override void OnInitialize()
        {
            repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            logger = LogManager.GetLogger(repository.Name, typeof(AppServer));

            // 启动服务
            Service.Instance.Default();

            //创建服务器网络监听
            _server = new ServerListener();

            logger.Info("AppServer Initialized!");
        }

        /// <summary>
        /// 启动服务器
        /// </summary>
        /// <param name="port"></param>
        public void StartServer(int port) 
        {
            _server.StartServer(port);

            logger.Info("AppServer Launched!");
        }

        /// <summary>
        /// 停止服务器
        /// </summary>
        public void StopServer()
        {
            _server.StopServer();
        }

        protected override void OnUpdate(float deltaTime)
        {
            _server.Update();
        }
    }
}
