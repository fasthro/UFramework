/*
 * @Author: fasthro
 * @Date: 2020-08-26 10:40:22
 * @Description: lua config
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UFramework.Serialize;
using UnityEngine;
using static ToLuaMenu;
using utils = UFramework.Utils;

namespace UFramework.Editor.Preferences
{
    /// <summary>
    /// Lua搜索路径
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
        /// path md5 value
        /// </summary>
        /// <value></value>
        [HideInInspector]
        public string pathMD5
        {
            get
            {
                Logger.Debug(path + " >>> " + utils.Str2MD5(path));
                return utils.Str2MD5(path);
            }
        }

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
    /// Wrap 类型
    /// </summary>
    [System.Serializable]
    public class LuaWrapBindTypeItem
    {
        /// <summary>
        /// 类名
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

    /// <summary>
    /// 构建文件
    /// </summary>
    [System.Serializable]
    public class LuaBuildFile
    {
        /// <summary>
        /// 原始路径
        /// </summary>
        public string sourcePath;

        /// <summary>
        /// 输出路径
        /// </summary>
        public string destPath;

        /// <summary>
        /// 文件长度
        /// </summary>
        public long len;

        /// <summary>
        /// hash
        /// </summary>
        public string hash;
    }

    public class LuaBuildSerdata : ISerializable
    {
        public SerializableType serializableType { get { return SerializableType.Editor; } }

        /// <summary>
        /// 文件列表
        /// </summary>
        /// <typeparam name="LuaBuildFile"></typeparam>
        /// <returns></returns>
        public List<LuaBuildFile> files = new List<LuaBuildFile>();

        public void Serialization()
        {
            Serializable<LuaBuildSerdata>.Serialization(this);
        }
    }
}