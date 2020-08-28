/*
 * @Author: fasthro
 * @Date: 2020-08-26 10:40:22
 * @Description: lua config
 */
using System;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using static ToLuaMenu;

namespace UFramework.Editor.Preferences
{
    /// <summary>
    /// lua search path item
    /// </summary>
    [System.Serializable]
    public class LuaSearchPathItem
    {
        /// <summary>
        /// 资源路径/资源目录
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup]
        [ValidateInput("ValidateInputPath", "$validateInputPathError", InfoMessageType.Error)]
        [FolderPath]
        public string path;

        /// <summary>
        /// 用于排序
        /// </summary>
        [HideInInspector]
        public int order = 0;

        #region 路径验证
        private string validateInputPathError;
        private bool ValidateInputPath(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                validateInputPathError = "error: path is empty.";
            }
            else if (!IOPath.FileExists(value) && !IOPath.DirectoryExists(value))
            {
                validateInputPathError = "error: path not exists.";
            }
            return !string.IsNullOrEmpty(value) && (IOPath.FileExists(value) || IOPath.DirectoryExists(value));
        }

        #endregion
    }

    /// <summary>
    /// Lua Wrap Bind Type Item
    /// </summary>
    [System.Serializable]
    public class LuaWrapBindTypeItem
    {
        /// <summary>
        /// Class Name
        /// </summary>
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup]
        [ValidateInput("ValidateClassName", "$validateClassNameError", InfoMessageType.Error)]
        public string className;

        #region 验证

        private string validateClassNameError;
        private bool ValidateClassName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                validateClassNameError = "class name is empty. please input class name.";
                return false;
            }
            var result = GetClassType(value) != null;
            if (!result) validateClassNameError = "[" + value + "] class not find.";
            return result;
        }

        #endregion

        /// <summary>
        /// BindType
        /// </summary>
        /// <value></value>
        [HideInInspector]
        public BindType bindType
        {
            get
            {
                return new BindType(GetClassType(className));
            }
        }

        /// <summary>
        /// 获取 Class Name 类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private Type GetClassType(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var assembly = Assembly.Load("Assembly-CSharp");
                var targetType = assembly.GetType(value);
                return targetType;
            }
            return null;
        }
    }
}