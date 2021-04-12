﻿// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/02/26 14:25
// * @Description:
// --------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Core;
using UFramework.Editor.Preferences.AssetBundle;
using UnityEngine;

namespace UFramework.Editor.Preferences.BuildFiles
{
    public class BuildFilesPage : IPage, IPageBar
    {
        public string menuName => "Build Files";

        static Preferences_SingleFileConfig Config => Serializer<Preferences_SingleFileConfig>.Instance;

        [ShowInInspector] [TableList(AlwaysExpanded = true, IsReadOnly = true)] [LabelText("File(Table Bytes)")]
        public List<SingleFileInfo> tableBytes = new List<SingleFileInfo>();

        [ShowInInspector] [TableList(AlwaysExpanded = true, IsReadOnly = true)] [LabelText("File(Proto PB)")]
        public List<SingleFileInfo> protos = new List<SingleFileInfo>();

        [ShowInInspector] [TableList(AlwaysExpanded = true)] [LabelText("File")]
        public List<SingleFileInfo> files = new List<SingleFileInfo>();

        private static string BuildFilesRootDir;

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            BuildFilesRootDir = IOPath.PathCombine(UApplication.TempDirectory, "Files");

            // table bytes
            CheckPath(ref tableBytes, IOPath.PathCombine(UApplication.AssetsDirectory, "Table/Data"), "Table", "*.bytes");
            // pb
            CheckPath(ref protos, IOPath.PathCombine(Environment.CurrentDirectory, "Assets/Scripts/Automatic/PB"), "PB",
                "*.pb");

            // files
            files.Clear();

            foreach (var file in Config.files)
                if (!file.isBuiltIn)
                    files.Add(file);
        }

        public void OnSaveDescribe()
        {
            Config.files.Clear();
            Config.files.AddRange(tableBytes);
            Config.files.AddRange(protos);
            Config.files.AddRange(files);
            Config.Serialize();
        }

        public void OnPageBarDraw()
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Build")))
            {
                Build(false);
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Clean Build")))
            {
                Build(true);
            }
        }

        private void CheckPath(ref List<SingleFileInfo> list, string dir, string targetDir, string seachPattern)
        {
            list.Clear();

            var pbFiles = IOPath.DirectoryGetFiles(dir, seachPattern, SearchOption.AllDirectories);
            foreach (var pbFile in pbFiles)
            {
                list.Add(new SingleFileInfo()
                {
                    path = IOPath.PathUnitySeparator(IOPath.PathRelativeAsset(pbFile)),
                    buildDirectory = targetDir,
                    isBuiltIn = true
                });
            }
        }

        public void Build(bool isCleanBuild)
        {
            if (isCleanBuild)
                IOPath.DirectoryClear(BuildFilesRootDir);

            OnSaveDescribe();

            foreach (var file in Config.files)
            {
                var fileName = IOPath.FileName(file.path, true);
                if (string.IsNullOrEmpty(file.buildDirectory))
                    IOPath.FileCopy(file.path, IOPath.PathCombine(BuildFilesRootDir, fileName));
                else
                    IOPath.FileCopy(file.path, IOPath.PathCombine(BuildFilesRootDir, file.buildDirectory, fileName));
            }
        }
    }
}