/*
 * @Author: fasthro
 * @Date: 2020-05-26 22:39:27
 * @Description: TCPManager(Socket Manager)
 */
using UFramework.Pool;
using UFramework.Network;
using System.Collections.Generic;
using Google.Protobuf;
using LuaInterface;

namespace UFramework
{
    public class TCPManager : BaseManager
    {
        /// <summary>
        /// Socket Client
        /// </summary>
        private SocketClient m_client;

        /// <summary>
        /// 消息处理队列
        /// </summary>
        /// <typeparam name="SocketEventArgs"></typeparam>
        /// <returns></returns>
        private Queue<SocketEventArgs> m_socketEventArgQueue = new Queue<SocketEventArgs>();

        /// <summary>
        /// 会话记录保证发送与接收一一对应
        /// </summary>
        /// <typeparam name="int"></typeparam>
        /// <typeparam name="int"></typeparam>
        /// <returns></returns>
        private Dictionary<int, int> m_sessionDictionary = new Dictionary<int, int>();

        /// <summary>
        /// 是否连接
        /// </summary>
        /// <value></value>
        public bool isConnected
        {
            get
            {
                return m_client != null && m_client.isConnected && m_client.isSocketConnected;
            }
        }

        protected override void OnInitialize()
        {
            // 初始化对象池
            ObjectPool<SocketEventArgs>.Instance.Initialize(10, 10);

            m_client = new SocketClient(OnSocketEventCallback);
        }

        /// <summary>
        /// 回调处理
        /// </summary>
        /// <param name="args"></param>
        private void OnSocketEventCallback(SocketEventArgs args)
        {
            m_socketEventArgQueue.Enqueue(args);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (m_client != null) m_client.Update();

            if (m_socketEventArgQueue.Count > 0)
            {
                var args = m_socketEventArgQueue.Dequeue();

                switch (args.socketState)
                {
                    case SocketState.Received:
                        var pack = args.socketPack;
                        if (m_sessionDictionary.ContainsKey(pack.cmd))
                        {
                            pack.valid = pack.sessionId == m_sessionDictionary[pack.cmd];
                        }
                        else
                        {
                            pack.valid = true;
                        }
                        break;
                    case SocketState.Connected:
                        break;
                    case SocketState.Disconnected:
                        break;
                    default:
                        break;
                }
                args.Recycle();
            }
        }

        public void Connecte(string ip, int port)
        {
            if (!isConnected)
            {
                m_client.Connect(ip, port);
            }
        }

        public void Send(SocketPack pack)
        {
            if (isConnected)
            {
                var sessionId = m_client.Send(pack);
                if (!m_sessionDictionary.ContainsKey(pack.cmd))
                {
                    m_sessionDictionary.Add(pack.cmd, sessionId);
                }
                else
                {
                    m_sessionDictionary[pack.cmd] = sessionId;
                }
            }
        }

        public void Send(int cmd, IMessage message)
        {
            if (isConnected)
            {
                var sessionId = m_client.Send(cmd, message);
                if (!m_sessionDictionary.ContainsKey(cmd))
                {
                    m_sessionDictionary.Add(cmd, sessionId);
                }
                else
                {
                    m_sessionDictionary[cmd] = sessionId;
                }
            }
        }

        public void Send(int cmd, LuaByteBuffer luabyte)
        {
            Send(SocketPackFactory.CreateWriter(cmd, luabyte));
        }

        protected override void OnLateUpdate()
        {

        }

        protected override void OnFixedUpdate()
        {

        }

        protected override void OnDispose()
        {
        }
    }
}