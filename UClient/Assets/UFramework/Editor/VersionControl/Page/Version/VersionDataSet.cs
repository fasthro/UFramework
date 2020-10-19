/*
 * @Author: fasthro
 * @Date: 2020-10-14 11:57:48
 * @Description: config
 */
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UFramework.Config;
using UFramework.VersionControl;
using UnityEngine;

namespace UFramework.Editor.VersionControl
{
    public class VersionControl_VersionConfig : IConfigObject
    {
        public string name { get { return "VersionControl_VersionConfig"; } }
        public FileAddress address { get { return FileAddress.Editor; } }

        /// <summary>
        /// 当前版本
        /// </summary>
        public int version = 0;

        /// <summary>
        /// 最低支持版本
        /// </summary>
        public int minVersion = 0;

        /// <summary>
        /// 基础资源数量
        /// </summary>
        public int baseResCount = 0;

        /// <summary>
        /// bundle files
        /// </summary>
        /// <typeparam name="FileInfo"></typeparam>
        /// <returns></returns>
        public List<VFile> bundleFiles = new List<VFile>();

        /// <summary>
        /// 当前版本补丁列表
        /// </summary>
        /// <returns></returns>
        public List<VPatch> patches = new List<VPatch>();

        /// <summary>
        /// 支持的版本信息列表
        /// </summary>
        /// <returns></returns>
        public List<VersionInfo> supports = new List<VersionInfo>();

        /// <summary>
        /// 历史版本信息列表
        /// </summary>
        /// <value></value>
        public List<VersionInfo> historys = new List<VersionInfo>();

        public void Save()
        {
            UConfig.Write<VersionControl_VersionConfig>(this);
        }
    }
}