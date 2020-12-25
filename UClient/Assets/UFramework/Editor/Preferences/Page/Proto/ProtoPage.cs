/*
 * @Author: fasthro
 * @Date: 2020-09-29 18:00:13
 * @Description: proto
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Core;
using UnityEditor;

namespace UFramework.Editor.Preferences.Proto
{
    public class ProtoPage : IPage, IPageBar
    {
        public string menuName { get { return "Proto"; } }

        static string CSOutpath;
        static string PBOutpath;
        static string ProtoDir;
        static string Protogen;
        static string PBServerOutpath;
        static string CSServerOutpath;

        static Preferences_Proto_Config Config { get { return Serializer<Preferences_Proto_Config>.Instance; } }

        [ShowInInspector]
        [ListDrawerSettings(Expanded = true, HideRemoveButton = true, HideAddButton = true)]
        public List<ProtoFile> protos = new List<ProtoFile>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            CSOutpath = IOPath.PathCombine(Environment.CurrentDirectory, "Assets/Scripts/Automatic/Protos");
            PBOutpath = IOPath.PathCombine(Environment.CurrentDirectory, "Assets/Scripts/Automatic/Lua/PB");

            var rootDir = IOPath.PathParent(Environment.CurrentDirectory);
            ProtoDir = IOPath.PathCombine(rootDir, "Protos");
            Protogen = IOPath.PathCombine(rootDir, "Tools", "protoc.exe");
            PBServerOutpath = IOPath.PathCombine(rootDir, "UServer/src/proto/pbc");
            CSServerOutpath = IOPath.PathCombine(rootDir, "LockstepServer/Proto/Src");

            protos.Clear();
            var files = IOPath.DirectoryGetFiles(ProtoDir, "*.proto", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                var fileName = IOPath.FileName(file);

                var proto = new ProtoFile();
                proto.name = fileName;
                proto.path = file;

                protos.Add(proto);
            }

            Dictionary<string, ProtoFile> protoDic = new Dictionary<string, ProtoFile>();
            for (int i = 0; i < Config.protos.Count; i++)
            {
                var proto = Config.protos[i];
                protoDic.Add(proto.path, proto);
            }

            List<int> removes = new List<int>();
            for (int i = 0; i < protos.Count; i++)
            {
                var proto = protos[i];
                ProtoFile op = null;
                if (protoDic.TryGetValue(proto.path, out op))
                {
                    protos[i].name = op.name;
                    protos[i].path = op.path;
                    protos[i].genType = op.genType;
                    protos[i].orderIndex = op.orderIndex;
                }

                protos[i].setPage(this);
            }

            for (int i = removes.Count - 1; i >= 0; i--)
            {
                protos.RemoveAt(i);
            }

            Sort();
            for (int i = 0; i < protos.Count; i++)
            {
                protos[i].orderIndex = i;
            }
            Sort();

            OnSaveDescribe();
        }

        public void OnPageBarDraw()
        {
            if (SirenixEditorGUI.ToolbarButton("Geberate All"))
            {
                IOPath.DirectoryClear(CSOutpath);
                IOPath.DirectoryClear(PBOutpath);
                IOPath.DirectoryClear(PBServerOutpath);

                foreach (var proto in protos)
                {
                    Compile(proto);
                }
                CreateLuaPBFile();
                CreateProtoCMDFile();
                AssetDatabase.Refresh();
            }
        }

        public void OnSaveDescribe()
        {
            Config.protos.Clear();
            Config.protos.AddRange(protos);
            Config.Serialize();
        }

        private void Sort()
        {
            protos.Sort((x, y) => x.orderIndex.CompareTo(y.orderIndex));
        }

        public void CreateLuaPBFile()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-- uframework automatically generated");
            sb.Append("local pb = {\n");
            foreach (var proto in protos)
            {
                if (proto.genType == ProtoGenerateType.All || proto.genType == ProtoGenerateType.PB)
                    sb.Append(string.Format("\t\"{0}\",", proto.name + ".pb"));
                sb.Append("\n");
            }
            sb.Append("}\n");
            sb.Append("return pb\n");

            IOPath.FileCreateText(IOPath.PathCombine(PBOutpath, "pb.lua"), sb.ToString(), null);
            // to server
            IOPath.FileCreateText(IOPath.PathCombine(PBServerOutpath, "pb.lua"), sb.ToString(), null);
        }

        public void CreateProtoCMDFile()
        {
            List<ProtoCMD> cmds = new List<ProtoCMD>();
            foreach (var proto in protos)
            {
                if (proto.genType == ProtoGenerateType.All || proto.genType == ProtoGenerateType.PB)
                {
                    string packName = "";
                    var lines = File.ReadAllLines(proto.path);
                    int lineIndex = 0;
                    while (lineIndex < lines.Length)
                    {
                        var line = lines[lineIndex];
                        if (string.IsNullOrEmpty(packName))
                        {
                            packName = ParseProtoPackage(line);
                        }
                        else
                        {
                            var matchIndex = MatchProtoCMDLine(line);
                            if (matchIndex != -1)
                            {
                                lineIndex++;
                                var protoCMD = ParseProtoCMDLine(line, lines[lineIndex], matchIndex);
                                if (protoCMD != null)
                                {
                                    protoCMD.package = packName;
                                    cmds.Add(protoCMD);
                                }
                            }
                        }
                        lineIndex++;
                    }
                }
            }

            WriteProtoCMDSFile(cmds, ProtoCMDType.C2S);
            WriteProtoCMDSFile(cmds, ProtoCMDType.S2C);
        }

        private string ParseProtoPackage(string line)
        {
            var match = Regex.Match(line, @"^package (?<name>.*);$");
            if (match.Success)
            {
                return match.Groups["name"].Value;
            }
            return null;
        }

        private int MatchProtoCMDLine(string line)
        {
            string[] patterns = new string[] {
                @"^//#\[C2S\]\[\d+\]#$",
                @"^//#\[S2C\]\[\d+\]#$",
                @"^//#\[C2S\]\[S2C\]\[\d+\]#$",
                @"^//#\[S2C\]\[C2S\]\[\d+\]#$" };

            for (int i = 0; i < patterns.Length; i++)
            {
                var match = Regex.Match(line, patterns[i]);
                if (match.Success)
                {
                    return i;
                }
            }
            return -1;
        }

        private ProtoCMD ParseProtoCMDLine(string cmdStr, string nameStr, int matchIndex)
        {
            var nameMatch = Regex.Match(nameStr, @"^message (?<name>.*) \{");
            if (nameMatch.Success)
            {
                ProtoCMD protoCMD = new ProtoCMD();
                Match match = null;
                if (matchIndex == 0)
                {
                    match = Regex.Match(cmdStr, @"^//#\[C2S\]\[(?<cmd>\d+)\]#$");
                    protoCMD.cmdType = ProtoCMDType.C2S;
                }
                else if (matchIndex == 1)
                {
                    match = Regex.Match(cmdStr, @"^//#\[S2C\]\[(?<cmd>\d+)\]#$");
                    protoCMD.cmdType = ProtoCMDType.S2C;
                }
                else if (matchIndex == 2)
                {
                    match = Regex.Match(cmdStr, @"^//#\[C2S\]\[S2C\]\[(?<cmd>\d+)\]#$");
                    protoCMD.cmdType = ProtoCMDType.All;
                }
                else if (matchIndex == 3)
                {
                    match = Regex.Match(cmdStr, @"^//#\[S2C\]\[C2S\]\[(?<cmd>\d+)\]#$");
                    protoCMD.cmdType = ProtoCMDType.All;
                }
                protoCMD.name = nameMatch.Groups["name"].Value;
                if (match != null && match.Success)
                {
                    protoCMD.cmd = int.Parse(match.Groups["cmd"].Value);
                }
                return protoCMD;
            }
            return null;
        }

        private void WriteProtoCMDSFile(List<ProtoCMD> cmds, ProtoCMDType cmdType)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-- uframework automatically generated");
            sb.Append("local cmds = {\n");

            for (int i = 0; i < cmds.Count; i++)
            {
                var cmd = cmds[i];
                if (cmd.cmdType == cmdType || cmd.cmdType == ProtoCMDType.All)
                {
                    sb.Append(string.Format("\t[{0}] = \"{1}.{2}\",\n", cmd.cmd, cmd.package, cmd.name));
                }
            }
            sb.Append("}\n");
            sb.Append("return cmds\n");

            var filename = cmdType.ToString().ToLower() + ".lua";
            IOPath.FileCreateText(IOPath.PathCombine(PBOutpath, filename), sb.ToString(), null);
            // to server
            IOPath.FileCreateText(IOPath.PathCombine(PBServerOutpath, filename), sb.ToString(), null);
        }

        public void UpOrder(ProtoFile proto)
        {
            var order = proto.orderIndex;
            if (order > 0)
            {
                var temp = protos[order - 1].orderIndex;
                protos[order - 1].orderIndex = proto.orderIndex;
                protos[order].orderIndex = temp;

                Sort();
            }
        }

        public void DownOrder(ProtoFile proto)
        {
            var order = proto.orderIndex;
            if (order < protos.Count - 1)
            {
                protos[order + 1].orderIndex = order;
                protos[order].orderIndex = order + 1;
            }
        }

        public void Compile(ProtoFile proto)
        {
            if (proto.genType == ProtoGenerateType.All)
            {
                CompileProtoCS(proto);
                CompileProtoPB(proto);
            }
            else if (proto.genType == ProtoGenerateType.Csharp)
            {
                CompileProtoCS(proto);
            }
            else if (proto.genType == ProtoGenerateType.PB)
            {
                CompileProtoPB(proto);
            }
        }

        static void CompileProtoCS(ProtoFile proto)
        {
            var outpath = IOPath.PathCombine(CSOutpath, proto.name + ".cs");
            if (!IOPath.DirectoryExists(CSOutpath))
                IOPath.DirectoryCreate(CSOutpath);

            IOPath.FileDelete(outpath);

            var incDir = Path.GetDirectoryName(proto.path);
            Utils.ExecuteProcess(Protogen, "--proto_path=" + incDir + " --csharp_out=" + CSOutpath + " " + proto.path, ProtoDir);
            // to server
            IOPath.FileCopy(outpath, IOPath.PathCombine(CSServerOutpath, proto.name + ".cs"));
        }


        static void CompileProtoPB(ProtoFile proto)
        {
            var outpath = IOPath.PathCombine(PBOutpath, proto.name + ".pb");
            if (!IOPath.DirectoryExists(PBOutpath))
                IOPath.DirectoryCreate(PBOutpath);

            IOPath.FileDelete(outpath);
            Utils.ExecuteProcess(Protogen, " -o " + outpath + " " + proto.path + " -I=" + ProtoDir, ProtoDir);
            // to skynet server
            IOPath.FileCopy(outpath, IOPath.PathCombine(PBServerOutpath, proto.name + ".pb"));
        }
    }
}