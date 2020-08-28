/*
 * @Author: fasthro
 * @Date: 2020-08-26 15:02:02
 * @Description: lua config
 */
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UFramework.Config;
using UnityEngine;

namespace UFramework.Lua
{
    public class LuaConfig : IConfigObject
    {
        public string name { get { return "LuaConfig"; } }

        /// <summary>
        /// lua search paths
        /// </summary>
        /// <value></value>
        public string[] searchPaths { get; set; }

        /// <summary>
        /// wrap class names
        /// </summary>
        /// <value></value>
        public string[] wrapClassNames { get; set; }

        public void Save()
        {
            UConfig.Write<LuaConfig>(this);
        }
    }
}
