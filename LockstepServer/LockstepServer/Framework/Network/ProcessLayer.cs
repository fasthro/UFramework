/*
 * @Author: fasthro
 * @Date: 2020/12/24 11:31:14
 * @Description:
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace LockstepServer
{
    public enum NetworkProcessLayer : short
    {
        All,
        Lua,
        CSharp
    }
}