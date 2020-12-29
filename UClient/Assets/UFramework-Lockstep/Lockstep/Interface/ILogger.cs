/*
 * @Author: fasthro
 * @Date: 2020/12/29 16:53:13
 * @Description:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lockstep
{
    public interface ILogger
    {
        void Debug(object message);

        void Info(object message);

        void Warning(object message);

        void Error(object message);
    }
}