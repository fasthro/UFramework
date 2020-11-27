/*
 * @Author: fasthro
 * @Date: 2020-09-29 18:00:13
 * @Description: proto
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Config;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Preferences
{
    public class ProtoPage : IPage, IPageBar
    {
        public string menuName { get { return "Proto"; } }
        static string CSOutpath;
        static string PBOutpath;
        static string ProtoDir;
        static string Protogen;
        static ProtoConfig describeObject;

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

            describeObject = UConfig.Read<ProtoConfig>();

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
            for (int i = 0; i < describeObject.protos.Count; i++)
            {
                var proto = describeObject.protos[i];
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

                foreach (var proto in protos)
                {
                    Compile(proto);
                }
                CreateLuaPBFile();
                AssetDatabase.Refresh();
            }
        }

        public void OnSaveDescribe()
        {
            describeObject.protos.Clear();
            describeObject.protos.AddRange(protos);
            describeObject.Save();
        }

        private void Sort()
        {
            protos.Sort((x, y) => x.orderIndex.CompareTo(y.orderIndex));
        }

        public void CreateLuaPBFile()
        {
            StringBuilder sb = new StringBuilder();
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
        }


        static void CompileProtoPB(ProtoFile proto)
        {
            var outpath = IOPath.PathCombine(PBOutpath, proto.name + ".pb");
            if (!IOPath.DirectoryExists(PBOutpath))
                IOPath.DirectoryCreate(PBOutpath);

            IOPath.FileDelete(outpath);
            Utils.ExecuteProcess(Protogen, " -o " + outpath + " " + proto.path + " -I=" + ProtoDir, ProtoDir);
        }
    }
}