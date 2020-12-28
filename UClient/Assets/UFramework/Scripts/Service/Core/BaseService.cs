/*
 * @Author: fasthro
 * @Date: 2020-05-25 23:52:22
 * @Description: BaseManager
 */

using System;
using LuaInterface;
using UnityEngine;

namespace UFramework
{
    public abstract class BaseService : BaseUnityBehaviourBindLua
    {
        public BaseService()
        {
            Initialize();
        }
    }
}