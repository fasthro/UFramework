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
        public string name;
        public long length;
        public string hash;
    }

    [System.Serializable]
    public class VPatch
    {
        public int aVersion;
        public int pVersion;
        public long timestamp;
        public VFile[] files = new VFile[0];
        public VScriptFile[] sFiles = new VScriptFile[0];

        public string name { get { return string.Format("p{0}.{1}.{2}", aVersion, pVersion, timestamp); } }
        public string fileName { get { return string.Format("{0}.zip", name); } }
    }

    [System.Serializable]
    public class VScriptFile : VFile
    {
        public int dirIndex;
    }

    [System.Serializable]
    public class VInfo
    {
        public int version;

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
        /// 版本脚本文件
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
                    file.length = reader.ReadInt64();
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
                    file.length = reader.ReadInt64();
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

                        // files
                        var pfnum = reader.ReadInt32();
                        patch.files = new VFile[pfnum];
                        for (int p = 0; p < pfnum; p++)
                        {
                            VFile file = new VFile();
                            file.name = reader.ReadString();
                            file.length = reader.ReadInt64();
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
                            file.length = reader.ReadInt64();
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
                    writer.Write(file.length);
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
                    writer.Write(item.length);
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

                        // files
                        writer.Write(patch.files.Length);
                        for (int k = 0; k < patch.files.Length; k++)
                        {
                            var file = patch.files[k];
                            writer.Write(file.name);
                            writer.Write(file.length);
                            writer.Write(file.hash);
                        }

                        // sfiles
                        writer.Write(patch.sFiles.Length);
                        for (int k = 0; k < patch.sFiles.Length; k++)
                        {
                            var file = patch.sFiles[k];
                            writer.Write(file.name);
                            writer.Write(file.length);
                            writer.Write(file.hash);
                            writer.Write(file.dirIndex);
                        }
                    }
                }
            }
        }

        public static List<VPatch> GetDownloadPatchs(Version ver, string targetPath)
        {
            var versionInfo = ver.GetVersionInfo(UConfig.Read<AppConfig>().version);
            List<VFile> downloadFils = new List<VFile>();
            for (int i = 0; i < ver.files.Length; i++)
            {
                var file = ver.files[i];
                var fp = IOPath.PathCombine(targetPath, file.name);
                if (!IOPath.FileExists(fp))
                {
                    downloadFils.Add(file);
                }
                else
                {
                    using (var stream = File.OpenRead(fp))
                    {
                        if (stream.Length != file.length)
                        {
                            downloadFils.Add(file);
                        }
                        else if (!HashUtils.GetCRC32Hash(stream).Equals(file.hash, StringComparison.OrdinalIgnoreCase))
                        {
                            downloadFils.Add(file);
                        }
                    }
                }
            }
            Debug.Log("download file count: " + downloadFils.Count);
            Dictionary<string, VPatch> map = new Dictionary<string, VPatch>();
            for (int i = 0; i < downloadFils.Count; i++)
            {
                var dfile = downloadFils[i];
                for (int k = 0; k < versionInfo.patchs.Length; k++)
                {
                    var patch = versionInfo.patchs[k];
                    var isChecked = false;
                    for (int n = 0; n < patch.files.Length; n++)
                    {
                        var pfile = patch.files[n];
                        if (dfile.name.Equals(pfile.name))
                        {
                            VPatch npatch = null;
                            if (map.TryGetValue(dfile.name, out npatch))
                            {
                                if (patch.timestamp > npatch.timestamp)
                                    map[dfile.name] = patch;
                            }
                            else map.Add(dfile.name, patch);
                            isChecked = true;
                            break;
                        }
                        if (isChecked) break;
                    }
                }
            }

            List<VPatch> downloads = new List<VPatch>();
            HashSet<int> dpVersions = new HashSet<int>();
            foreach (var item in map)
            {
                if (!dpVersions.Contains(item.Value.pVersion))
                {
                    Debug.Log(string.Format("download patch v{0}.{1}", item.Value.aVersion, item.Value.pVersion));
                    downloads.Add(item.Value);
                }
            }

            return downloads;
        }
    }
}