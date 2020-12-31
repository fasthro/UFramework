/*
 * @Author: fasthro
 * @Date: 2020/12/18 15:28:02
 * @Description:
 */

using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LockstepServer
{
    public class HelperService : BaseService, IHelperService
    {
        public override void Initialize()
        {
            LogHelper.Initialize();
        }
    }
}