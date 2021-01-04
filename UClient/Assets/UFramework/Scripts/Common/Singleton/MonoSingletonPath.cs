/*
 * @Author: fasthro
 * @Date: 2019-06-19 17:39:24
 * @Description: 
 */

using System;

namespace UFramework
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MonoSingletonPath : Attribute
    {
        public string pathInHierarchy { get; private set; }

        public MonoSingletonPath(string pathInHierarchy)
        {
            this.pathInHierarchy = pathInHierarchy;
        }

    }
}
