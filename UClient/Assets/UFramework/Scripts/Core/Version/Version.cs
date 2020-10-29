/*
 * @Author: fasthro
 * @Date: 2020-10-13 17:35:43
 * @Description: version
 */
using System;
using System.Collections.Generic;
using System.IO;
using UFramework.Config;
using UnityEngine;

namespace UFramework.VersionControl
{
    [System.Serializable]
    public class VFile
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string name;

        /// <summary>
        /// 文件长度
        /// </summary>
        public long len;

        /// <summary>
        /// 文件CRC32哈希
        /// </summary>
        public string hash;
    }

    [System.Serializable]
    public class VPatch
    {
        /// <summary>
        /// 应用版本
        /// </summary>
        public int aVersion;

        /// <summary>
        /// 补丁版本
        /// </summary>
        public int pVersion;

        /// <summary>
        /// 时间戳
        /// </summary>
        public long timestamp;

        /// <summary>
        /// 补丁文件大小
        /// </summary>
        public long len;

        /// <summary>
        /// 资源文件列表
        /// </summary>
        public VFile[] files = new VFile[0];

        /// <summary>
        /// 代码文件列表
        /// </summary>
        public VScriptFile[] sFiles = new VScriptFile[0];

        // 补丁文件名称
        public string fileName { get { return string.Format("p{0}.{1}.{2}.zip", aVersion, pVersion, timestamp); } }

        public string key { get { return string.Format("{0}.{1}", aVersion, pVersion); } }
    }

    [System.Serializable]
    public class VScriptFile : VFile
    {
        /// <summary>
        /// 目录索引
        /// </summary>
        public int dirIndex;

        // 文件表示 key
        public string key { get { return string.Format("{0}-{1}", dirIndex, name); } }
    }

    [System.Serializable]
    public class VInfo
    {
        /// <summary>
        /// 应用版本
        /// </summary>
        public int version;

        /// <summary>
        /// 补丁列表
        /// </summary>
        public VPatch[] patchs = new VPatch[0];
    }

    public class Version
    {
        // 版本文件名称
        readonly public static string FileName = "version";

        /// <summary>
        /// 当前版本
        /// </summary>
        public int version { get; set; }

        /// <summary>
        /// 最低支持版本
        /// </summary>
        public int minVersion { get; set; }

        /// <summary>
        /// 版本时间戳
        /// </summary>
        public long timestamp { get; set; }

        /// <summary>
        /// 版本资源文件列表
        /// </summary>
        /// <typeparam name="FileInfo"></typeparam>
        /// <returns></returns>
        public VFile[] files = new VFile[0];

        /// <summary>
        /// 版本脚本目录
        /// </summary>
        public string[] sDirs = new string[0];

        /// <summary>
        /// 版本脚本文件列表
        /// </summary>
        /// <typeparam name="VScriptFile"></typeparam>
        /// <returns></returns>
        public VScriptFile[] sFiles = new VScriptFile[0];

        /// <summary>
        /// 版本信息列表
        /// </summary>
        /// <value></value>
        public Dictionary<int, VInfo> versions = new Dictionary<int, VInfo>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_version"></param>
        /// <returns></returns>
        public VInfo GetVersionInfo(int _version)
        {
            VInfo info = null;
            versions.TryGetValue(_version, out info);
            return info;
        }

        public static Version LoadVersion(string path)
        {
            if (!IOPath.FileExists(path)) return null;

            var ver = new Version();
            using (var stream = File.OpenRead(path))
            {
                var reader = new BinaryReader(stream);
                // base
                ver.version = reader.ReadInt32();
                ver.minVersion = reader.ReadInt32();
                ver.timestamp = reader.ReadInt64();

                // files
                var fnum = reader.ReadInt32();
                ver.files = new VFile[fnum];
                for (int i = 0; i < fnum; i++)
                {
                    var file = new VFile();
                    file.name = reader.ReadString();
                    file.len = reader.ReadInt64();
                    file.hash = reader.ReadString();
                    ver.files[i] = file;
                }

                // sdir
                var dnum = reader.ReadInt32();
                ver.sDirs = new string[dnum];
                for (int i = 0; i < dnum; i++)
                    ver.sDirs[i] = reader.ReadString();

                // sfile
                var snum = reader.ReadInt32();
                ver.sFiles = new VScriptFile[snum];
                for (int i = 0; i < snum; i++)
                {
                    VScriptFile file = new VScriptFile();
                    file.name = reader.ReadString();
                    file.len = reader.ReadInt64();
                    file.hash = reader.ReadString();
                    file.dirIndex = reader.ReadInt32();
                    ver.sFiles[i] = file;
                }

                // versions
                var count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    var info = new VInfo();
                    info.version = reader.ReadInt32();
                    var pnum = reader.ReadInt32();
                    info.patchs = new VPatch[pnum];
                    for (int k = 0; k < pnum; k++)
                    {
                        var patch = new VPatch();
                        patch.aVersion = reader.ReadInt32();
                        patch.pVersion = reader.ReadInt32();
                        patch.timestamp = reader.ReadInt64();
                        patch.len = reader.ReadInt64();

                        // files
                        var pfnum = reader.ReadInt32();
                        patch.files = new VFile[pfnum];
                        for (int p = 0; p < pfnum; p++)
                        {
                            VFile file = new VFile();
                            file.name = reader.ReadString();
                            file.len = reader.ReadInt64();
                            file.hash = reader.ReadString();
                            patch.files[p] = file;
                        }

                        // sfiles
                        var psfnum = reader.ReadInt32();
                        patch.sFiles = new VScriptFile[psfnum];
                        for (int p = 0; p < psfnum; p++)
                        {
                            VScriptFile file = new VScriptFile();
                            file.name = reader.ReadString();
                            file.len = reader.ReadInt64();
                            file.hash = reader.ReadString();
                            file.dirIndex = reader.ReadInt32();
                            patch.sFiles[p] = file;
                        }
                        info.patchs[k] = patch;
                    }
                    ver.versions.Add(info.version, info);
                }
            }
            return ver;
        }

        public static void VersionWrite(string path, Version ver)
        {
            using (var stream = File.OpenWrite(path))
            {
                var writer = new BinaryWriter(stream);
                // base
                writer.Write(ver.version);
                writer.Write(ver.minVersion);
                writer.Write(ver.timestamp);

                // files
                writer.Write(ver.files.Length);
                for (int i = 0; i < ver.files.Length; i++)
                {
                    var file = ver.files[i];
                    writer.Write(file.name);
                    writer.Write(file.len);
                    writer.Write(file.hash);
                }

                // sdir
                writer.Write(ver.sDirs.Length);
                for (int i = 0; i < ver.sDirs.Length; i++)
                    writer.Write(ver.sDirs[i]);

                // sfile
                writer.Write(ver.sFiles.Length);
                foreach (var item in ver.sFiles)
                {
                    writer.Write(item.name);
                    writer.Write(item.len);
                    writer.Write(item.hash);
                    writer.Write(item.dirIndex);
                }

                // versions
                writer.Write(ver.versions.Count);
                foreach (var item in ver.versions)
                {
                    var info = item.Value;
                    writer.Write(info.version);

                    writer.Write(info.patchs.Length);
                    for (int i = 0; i < info.patchs.Length; i++)
                    {
                        var patch = info.patchs[i];
                        writer.Write(patch.aVersion);
                        writer.Write(patch.pVersion);
                        writer.Write(patch.timestamp);
                        writer.Write(patch.len);

                        // files
                        writer.Write(patch.files.Length);
                        for (int k = 0; k < patch.files.Length; k++)
                        {
                            var file = patch.files[k];
                            writer.Write(file.name);
                            writer.Write(file.len);
                            writer.Write(file.hash);
                        }

                        // sfiles
                        writer.Write(patch.sFiles.Length);
                        for (int k = 0; k < patch.sFiles.Length; k++)
                        {
                            var file = patch.sFiles[k];
                            writer.Write(file.name);
                            writer.Write(file.len);
                            writer.Write(file.hash);
                            writer.Write(file.dirIndex);
                        }
                    }
                }
            }
        }
    }
}