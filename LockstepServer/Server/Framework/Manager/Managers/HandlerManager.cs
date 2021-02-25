// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using LiteNetLib;
using System;
using System.Collections.Generic;

namespace UFramework
{
    public class HandlerManager : BaseManager, IHandlerManager
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

        public void OnReceive(NetPeer peer, int cmd, int session, NetworkProcessLayer layer, byte[] data)
        {
            if (handlerDict.TryGetValue(cmd, out var handler))
            {
                try
                {
                    handler.DoMessage(peer, session, layer, data);
                    if (handler.DoResponse())
                    {
                        _networkManager.Send(peer, handler.cmd, handler.session, handler.layer, handler.responseMessage);
                    }

                    handler.DoAction();
                }
                catch (Exception ex)
                {
                    var s2c = new Lockstep.Message.ServerException {Cmd = cmd, Session = session};
                    _networkManager.Send(peer, GameConst.NETWORK_CMD_SERVER_EXCEPTION, 0, NetworkProcessLayer.All, s2c);
                    LogHelper.Error(ex.Message + "\n" + ex.StackTrace);
                }
            }
        }
    }
}