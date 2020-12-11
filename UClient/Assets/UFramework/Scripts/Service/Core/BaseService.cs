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
    public abstract class BaseService : ILifeCycle
    {
        public BaseService()
        {
            OnAwake();
        }

        public virtual void OnAwake()
        {

        }

        public virtual void OnStart()
        {

        }

        public virtual void OnUpdate(float deltaTime)
        {

        }

        public virtual void OnLateUpdate()
        {
        }

        public virtual void OnFixedUpdate()
        {

        }

        public virtual void OnDestroy()
        {

        }

        public virtual void OnApplicationQuit()
        {

        }
    }
}