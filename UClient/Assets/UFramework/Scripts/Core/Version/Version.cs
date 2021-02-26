// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-10-13 17:35:43
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

namespace UFramework.Core
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

        /// <summary>
        /// Build文件列表
        /// </summary>
        public VBuildFile[] bFiles = new VBuildFile[0];

        // 补丁文件名称
        public string fileName => $"p{aVersion}.{pVersion}.{timestamp}.zip";

        public string key => $"{aVersion}.{pVersion}";
    }

    [System.Serializable]
    public class VScriptFile : VFile
    {
        /// <summary>
        /// 目录索引
        /// </summary>
        public int dirIndex;

        // 文件表示 key
        public string key => $"{dirIndex}-{name}";
    }

    [System.Serializable]
    public class VBuildFile : VFile
    {
        /// <summary>
        /// 目录索引
        /// </summary>
        public int dirIndex;

        // 文件表示 key
        public string key => $"{dirIndex}-{name}";
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
        public static readonly string FileName = "version";

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
        /// 版本Build文件目录
        /// </summary>
        public string[] bDirs = new string[0];

        /// <summary>
        /// 版本Build文件列表
        /// </summary>
        /// <typeparam name="VScriptFile"></typeparam>
        /// <returns></returns>
        public VBuildFile[] bFiles = new VBuildFile[0];

        /// <summary>
        /// 版本信息列表
        /// </summary>
        /// <value></value>
        public Dictionary<int, VInfo> versionDict = new Dictionary<int, VInfo>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_version"></param>
        /// <returns></returns>
        public VInfo GetVersionInfo(int _version)
        {
            versionDict.TryGetValue(_version, out var info);
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
                for (var i = 0; i < fnum; i++)
                {
                    var file = new VFile
                    {
                        name = reader.ReadString(),
                        len = reader.ReadInt64(),
                        hash = reader.ReadString()
                    };
                    ver.files[i] = file;
                }

                // script dirs
                var sdnum = reader.ReadInt32();
                ver.sDirs = new string[sdnum];
                for (var i = 0; i < sdnum; i++)
                    ver.sDirs[i] = reader.ReadString();

                // script files
                var snum = reader.ReadInt32();
                ver.sFiles = new VScriptFile[snum];
                for (var i = 0; i < snum; i++)
                {
                    var file = new VScriptFile
                    {
                        name = reader.ReadString(),
                        len = reader.ReadInt64(),
                        hash = reader.ReadString(),
                        dirIndex = reader.ReadInt32()
                    };
                    ver.sFiles[i] = file;
                }

                // build dirs
                var bdnum = reader.ReadInt32();
                ver.bDirs = new string[bdnum];
                for (var i = 0; i < bdnum; i++)
                    ver.bDirs[i] = reader.ReadString();

                // build files
                var bnum = reader.ReadInt32();
                ver.bFiles = new VBuildFile[bnum];
                for (var i = 0; i < bnum; i++)
                {
                    var file = new VBuildFile
                    {
                        name = reader.ReadString(),
                        len = reader.ReadInt64(),
                        hash = reader.ReadString(),
                        dirIndex = reader.ReadInt32()
                    };
                    ver.bFiles[i] = file;
                }

                // versions
                var count = reader.ReadInt32();
                for (var i = 0; i < count; i++)
                {
                    var info = new VInfo
                    {
                        version = reader.ReadInt32()
                    };
                    var pnum = reader.ReadInt32();
                    info.patchs = new VPatch[pnum];
                    for (var k = 0; k < pnum; k++)
                    {
                        var patch = new VPatch
                        {
                            aVersion = reader.ReadInt32(),
                            pVersion = reader.ReadInt32(),
                            timestamp = reader.ReadInt64(),
                            len = reader.ReadInt64()
                        };

                        // files
                        var pfnum = reader.ReadInt32();
                        patch.files = new VFile[pfnum];
                        for (var p = 0; p < pfnum; p++)
                        {
                            var file = new VFile
                            {
                                name = reader.ReadString(),
                                len = reader.ReadInt64(),
                                hash = reader.ReadString()
                            };
                            patch.files[p] = file;
                        }

                        // script files
                        var psfnum = reader.ReadInt32();
                        patch.sFiles = new VScriptFile[psfnum];
                        for (var p = 0; p < psfnum; p++)
                        {
                            var file = new VScriptFile
                            {
                                name = reader.ReadString(),
                                len = reader.ReadInt64(),
                                hash = reader.ReadString(),
                                dirIndex = reader.ReadInt32()
                            };
                            patch.sFiles[p] = file;
                        }

                        // build files
                        var pbfnum = reader.ReadInt32();
                        patch.bFiles = new VBuildFile[pbfnum];
                        for (var p = 0; p < pbfnum; p++)
                        {
                            var file = new VBuildFile()
                            {
                                name = reader.ReadString(),
                                len = reader.ReadInt64(),
                                hash = reader.ReadString(),
                                dirIndex = reader.ReadInt32()
                            };
                            patch.bFiles[p] = file;
                        }

                        info.patchs[k] = patch;
                    }

                    ver.versionDict.Add(info.version, info);
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
                for (var i = 0; i < ver.files.Length; i++)
                {
                    var file = ver.files[i];
                    writer.Write(file.name);
                    writer.Write(file.len);
                    writer.Write(file.hash);
                }

                // script dirs
                writer.Write(ver.sDirs.Length);
                for (var i = 0; i < ver.sDirs.Length; i++)
                    writer.Write(ver.sDirs[i]);

                // script files
                writer.Write(ver.sFiles.Length);
                foreach (var item in ver.sFiles)
                {
                    writer.Write(item.name);
                    writer.Write(item.len);
                    writer.Write(item.hash);
                    writer.Write(item.dirIndex);
                }

                // build dirs
                writer.Write(ver.bDirs.Length);
                for (var i = 0; i < ver.bDirs.Length; i++)
                    writer.Write(ver.bDirs[i]);

                // build files
                writer.Write(ver.bFiles.Length);
                foreach (var item in ver.bFiles)
                {
                    writer.Write(item.name);
                    writer.Write(item.len);
                    writer.Write(item.hash);
                    writer.Write(item.dirIndex);
                }

                // versions
                writer.Write(ver.versionDict.Count);
                foreach (var item in ver.versionDict)
                {
                    var info = item.Value;
                    writer.Write(info.version);

                    writer.Write(info.patchs.Length);
                    for (var i = 0; i < info.patchs.Length; i++)
                    {
                        var patch = info.patchs[i];
                        writer.Write(patch.aVersion);
                        writer.Write(patch.pVersion);
                        writer.Write(patch.timestamp);
                        writer.Write(patch.len);

                        // files
                        writer.Write(patch.files.Length);
                        for (var k = 0; k < patch.files.Length; k++)
                        {
                            var file = patch.files[k];
                            writer.Write(file.name);
                            writer.Write(file.len);
                            writer.Write(file.hash);
                        }

                        // script files
                        writer.Write(patch.sFiles.Length);
                        for (var k = 0; k < patch.sFiles.Length; k++)
                        {
                            var file = patch.sFiles[k];
                            writer.Write(file.name);
                            writer.Write(file.len);
                            writer.Write(file.hash);
                            writer.Write(file.dirIndex);
                        }

                        // script files
                        writer.Write(patch.bFiles.Length);
                        for (var k = 0; k < patch.bFiles.Length; k++)
                        {
                            var file = patch.bFiles[k];
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