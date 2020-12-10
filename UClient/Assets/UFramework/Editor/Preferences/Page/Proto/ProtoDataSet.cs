/*
 * @Author: fasthro
 * @Date: 2020-11-30 14:23:23
 * @Description: proto dataset
 */
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UFramework.Serialize;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Preferences
{
    /// <summary>
    /// Proto 生成类型
    /// </summary>
    public enum ProtoGenerateType
    {
        All,
        Csharp,
        PB
    }

    /// <summary>
    /// proto file
    /// </summary>
    [System.Serializable]
    public class ProtoFile
    {
        /// <summary>
        /// 名称
        /// </summary>
        [ReadOnly, HideLabel, HorizontalGroup]
        public string name;

        /// <summary>
        /// 生成类型
        /// </summary>
        [HideLabel, HorizontalGroup(200)]
        public ProtoGenerateType genType;

        /// <summary>
        /// 路径
        /// </summary>
        [HideInInspector]
        public string path;

        /// <summary>
        /// 顺序
        /// </summary>
        [HideInInspector]
        public int orderIndex;

        /// <summary>
        /// 生成方法
        /// </summary>
        [HorizontalGroup(200)]
        [Button]
        public void Generate()
        {
            _page?.Compile(this);
            _page?.CreateLuaPBFile();

            AssetDatabase.Refresh();
        }

        [HorizontalGroup(25)]
        [Button("↑")]
        public void Up()
        {
            _page?.UpOrder(this);
        }

        [HorizontalGroup(25)]
        [Button("↓")]
        public void Down()
        {
            _page?.DownOrder(this);
        }

        [HideInInspector]
        private ProtoPage _page;

        public void setPage(ProtoPage page)
        {
            _page = page;
        }
    }

    public enum ProtoCMDType
    {
        C2S,
        S2C,
        All,
    }

    /// <summary>
    /// 命令
    /// </summary>
    public class ProtoCMD
    {
        public string package;
        public int cmd;
        public string name;
        public ProtoCMDType cmdType;
    }

    /// <summary>
    /// proto serdata
    /// </summary>
    public class ProtoSerdata : ISerializable
    {
        public SerializableType serializableType { get { return SerializableType.Editor; } }

        public List<ProtoFile> protos = new List<ProtoFile>();

        public void Serialization()
        {
            Serializable<ProtoSerdata>.Serialization(this);
        }
    }
}