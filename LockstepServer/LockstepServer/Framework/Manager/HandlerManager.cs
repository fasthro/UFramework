/*
 * @Author: fasthro
 * @Date: 2020/12/22 10:29:49
 * @Description:
 */

using LiteNetLib;
using System;
using System.Collections.Generic;

namespace LockstepServer
{
    public class HandlerManager : BaseManager
    {
        private static Dictionary<int, IHandler> handlerDict = new Dictionary<int, IHandler>();
        public NetManager netManager => Service.Instance.GetManager<NetManager>();

        public void RegisterHandler(int protocal, IHandler handler)
        {
            if (!handlerDict.ContainsKey(protocal))
            {
                handlerDict.Add(protocal, handler);
            }
            else
            {
                LogHelper.Warn($"重复注册 Handler {protocal}");
            }
        }

        public IHandler GetHandler(int protocal)
        {
            if (handlerDict.ContainsKey(protocal))
            {
                return handlerDict[protocal];
            }
            return null;
        }

        public void RemoveHandler(int protocal)
        {
            if (handlerDict.ContainsKey(protocal))
            {
                handlerDict.Remove(protocal);
            }
        }

        public void OnReceive(NetPeer peer, int cmd, int session, NetworkProcessLayer layer, byte[] data)
        {
            IHandler handler = null;
            if (handlerDict.TryGetValue(cmd, out handler))
            {
                try
                {
                    handler.DoMessage(peer, session, layer, data);
                    if (handler.DoResponse())
                    {
                        netManager.Send(peer, handler.cmd, handler.session, handler.layer, handler.responseMessage);
                    }
                    handler.DoAction();
                }
                catch (Exception ex)
                {
                    var s2c = new PB.ServerException();
                    s2c.Cmd = cmd;
                    s2c.Session = session;

                    netManager.Send(peer, NetwokCmd.SERVER_EXCEPTION, 0, NetworkProcessLayer.All, s2c);
                    LogHelper.Error(ex.Message + "\n" + ex.StackTrace);
                }
            }
        }
    }
}