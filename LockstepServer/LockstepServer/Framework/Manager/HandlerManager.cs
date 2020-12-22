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

        public void OnReceive(NetPeer peer, int cmd, int session, byte[] data)
        {
            IHandler handler = null;
            if (handlerDict.TryGetValue(cmd, out handler))
            {
                try
                {
                    handler?.OnMessage(peer, session, data);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message);
                }
            }
        }
    }
}