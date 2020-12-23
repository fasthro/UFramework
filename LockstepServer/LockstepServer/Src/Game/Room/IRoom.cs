/*
 * @Author: fasthro
 * @Date: 2020/12/23 11:49:00
 * @Description:
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer.Src
{
    public enum RoomStatus
    {
        Underfill,
        Ready,
    }

    internal interface IRoom
    {
        int roomId { get; }
        string secretKey { get; }
        RoomStatus status { get; }
        int count { get; }
    }
}