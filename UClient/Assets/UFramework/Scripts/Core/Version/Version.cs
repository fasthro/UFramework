/*
 * @Author: fasthro
 * @Date: 2020-10-13 17:35:43
 * @Description: version
 */
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
        public string hash = "";
    }

    [System.Serializable]
    public class VPatch
    {
        public int version;
        public Dictionary<string, VFile> files = new Dictionary<string, VFile>();
    }

    [System.Serializable]
    public class VersionInfo
    {
        [HideInInspector]
        public int version;

        [HideInInspector]
        public int baseResCount;

        public List<VPatch> patchs = new List<VPatch>();
    }

    public class Version
    {
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
        public List<VFile> files = new List<VFile>();

        /// <summary>
        /// 版本信息列表
        /// </summary>
        /// <value></value>
        public Dictionary<int, VersionInfo> versions = new Dictionary<int, VersionInfo>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_version"></param>
        /// <returns></returns>
        public VersionInfo GetVersionInfo(int _version)
        {
            VersionInfo info = null;
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

                ver.version = reader.ReadInt32();
                ver.minVersion = reader.ReadInt32();
                ver.timestamp = reader.ReadInt64();

                var fnum = reader.ReadInt32();
                for (int i = 0; i < fnum; i++)
                {
                    var file = new VFile();
                    file.name = reader.ReadString();
                    file.length = reader.ReadInt64();
                    file.hash = reader.ReadString();
                    ver.files.Add(file);
                }

                var count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    var info = new VersionInfo();
                    info.version = reader.ReadInt32();
                    info.baseResCount = reader.ReadInt32();
                    var num = reader.ReadInt32();
                    info.patchs.Clear();
                    for (int k = 0; k < num; k++)
                    {
                        var patch = new VPatch();
                        patch.version = reader.ReadInt32();

                        var pnum = reader.ReadInt32();
                        for (int p = 0; p < pnum; p++)
                        {
                            VFile fi = new VFile();
                            fi.name = reader.ReadString();
                            fi.length = reader.ReadInt64();
                            fi.hash = reader.ReadString();
                            patch.files.Add(fi.name, fi);
                        }
                        info.patchs.Add(patch);
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
                writer.Write(ver.version);
                writer.Write(ver.minVersion);
                writer.Write(ver.timestamp);

                writer.Write(ver.files.Count);
                for (int i = 0; i < ver.files.Count; i++)
                {
                    var file = ver.files[i];
                    writer.Write(file.name);
                    writer.Write(file.length);
                    writer.Write(file.hash);
                }

                writer.Write(ver.versions.Count);
                foreach (var item in ver.versions)
                {
                    var info = item.Value;
                    writer.Write(info.version);
                    writer.Write(info.baseResCount);

                    writer.Write(info.patchs.Count);
                    for (int i = 0; i < info.patchs.Count; i++)
                    {
                        var patch = info.patchs[i];
                        writer.Write(patch.version);
                        writer.Write(patch.files.Count);

                        foreach (var p in patch.files)
                        {
                            var file = p.Value;
                            writer.Write(file.name);
                            writer.Write(file.length);
                            writer.Write(file.hash);
                        }
                    }
                }
            }
        }

        public static List<VPatch> GetDownloadPatchs(Version ver)
        {
            var versionInfo = ver.GetVersionInfo(UConfig.Read<AppConfig>().version);
            List<VFile> downloadFils = new List<VFile>();
            for (int i = 0; i < ver.files.Count; i++)
            {
                var file = ver.files[i];
                var fp = IOPath.PathCombine(App.BundleDirectory, file.name);
                Debug.Log(">>> " + fp);
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
                        // TODO Hash
                    }
                }
            }
            Debug.Log("download file count: " + downloadFils.Count);
            Dictionary<string, VPatch> map = new Dictionary<string, VPatch>();
            for (int i = 0; i < downloadFils.Count; i++)
            {
                var file = downloadFils[i];
                for (int k = 0; k < versionInfo.patchs.Count; k++)
                {
                    var patch = versionInfo.patchs[k];
                    if (patch.files.ContainsKey(file.name))
                    {
                        VPatch npatch = null;
                        if (map.TryGetValue(file.name, out npatch))
                        {
                            if (patch.version > npatch.version)
                                map[file.name] = patch;
                        }
                        else map.Add(file.name, patch);
                    }
                }
            }

            List<VPatch> downloads = new List<VPatch>();
            HashSet<int> dpVersions = new HashSet<int>();
            foreach (var item in map)
            {
                if (!dpVersions.Contains(item.Value.version))
                    downloads.Add(item.Value);
            }

            return downloads;
        }
    }
}