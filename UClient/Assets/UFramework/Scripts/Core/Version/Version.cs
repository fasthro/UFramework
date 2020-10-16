/*
 * @Author: fasthro
 * @Date: 2020-10-13 17:35:43
 * @Description: version
 */
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UFramework.VersionControl
{
    [System.Serializable]
    public class PatchInfo
    {
        public int version;
        public int count;
        public long size;
        public string hash = "";
    }

    [System.Serializable]
    public class VersionInfo
    {
        [HideInInspector]
        public int version;

        [HideInInspector]
        public int baseResCount;

        public List<PatchInfo> patchs = new List<PatchInfo>();
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
                        var patch = new PatchInfo();
                        patch.version = reader.ReadInt32();
                        patch.count = reader.ReadInt32();
                        patch.size = reader.ReadInt64();
                        patch.hash = reader.ReadString();
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
                        writer.Write(patch.count);
                        writer.Write(patch.size);
                        writer.Write(patch.hash);
                    }
                }
            }
        }
    }
}