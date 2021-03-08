﻿// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/26 15:02
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UFramework.Core;
using UnityEngine;

namespace UFramework.Editor.Preferences.BuildFiles
{
    public enum SingleFileType
    {
        ProtoPB,
        File
    }

    [System.Serializable]
    public class SingleFileInfo
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        [ShowInInspector] [FilePath]
        public string path;

        /// <summary>
        /// 文件构建目标目录
        /// 如果为空默认SingleFile目录，否则其子目录
        /// </summary>
        [ShowInInspector] public string buildDirectory;
        
        /// <summary>
        /// 文件类型
        /// </summary>
        [HideInInspector] public SingleFileType fileType;
    }

    public class Preferences_SingleFileConfig : ISerializable
    {
        public SerializableAssigned assigned => SerializableAssigned.Editor;

        public List<SingleFileInfo> files = new List<SingleFileInfo>();

        public void Serialize()
        {
            Serializer<Preferences_SingleFileConfig>.Serialize(this);
        }
    }
}