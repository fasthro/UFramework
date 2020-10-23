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
    public class PublishRecord
    {
        public int version;
        public List<VFile> files = new List<VFile>();
    }

    public class PlatformVersion
    {
        /// <summary>
        /// platform
        /// </summary>
        public int platform;

        /// <summary>
        /// files
        /// </summary>
        /// <typeparam name="FileInfo"></typeparam>
        /// <returns></returns>
        public List<VFile> files = new List<VFile>();

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

        /// <summary>
        /// 发布版本文件记录
        /// </summary>
        /// <returns></returns>
        public List<PublishRecord> publishRecords = new List<PublishRecord>();
    }

    public class VersionControl_VersionConfig : IConfigObject
    {
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
        /// 平台版本信息
        /// </summary>
        /// <typeparam name="int"></typeparam>
        /// <typeparam name="PlatformVersion"></typeparam>
        /// <returns></returns>
        public List<PlatformVersion> platforms = new List<PlatformVersion>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PlatformVersion GetPV()
        {
            PlatformVersion pv = null;
            int platform = Platform.BuildTargetCurrent;
            for (int i = 0; i < platforms.Count; i++)
            {
                if (platforms[i].platform == platform)
                {
                    pv = platforms[i];
                    break;
                }
            }
            if (pv == null)
            {
                pv = new PlatformVersion();
                pv.platform = platform;
                platforms.Add(pv);
            }
            return pv;
        }

        public void Save()
        {
            UConfig.Write<VersionControl_VersionConfig>(this);
        }
    }
}