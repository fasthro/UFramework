// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-08-08 19:39:10
// * @Description:
// --------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using FairyGUI;
using LuaInterface;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Core;
using UnityEditor;
using UnityEngine;
using static ToLuaMenu;
using Debug = UnityEngine.Debug;

namespace UFramework.Editor.Preferences.Lua
{
    public class LuaPage : IPage, IPageBar
    {
        [ShowInInspector] [BoxGroup("General Setting")] [LabelText("Build Use Byte Encode")]
        public bool byteEncode = true;

        /// <summary>
        /// lua 搜索路径
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        [ShowInInspector] [ListDrawerSettings(Expanded = true, CustomRemoveElementFunction = "CustomRemoveElementFunction_searchPaths")]
        public List<LuaSearchPathItem> searchPaths = new List<LuaSearchPathItem>();

        /// <summary>
        /// lua bind type items
        /// </summary>
        /// <typeparam name="LuaWrapBindTypeItem"></typeparam>
        /// <returns></returns>
        [ShowInInspector] public List<LuaWrapBindTypeItem> wrapBindTypes = new List<LuaWrapBindTypeItem>();

        public string menuName
        {
            get { return "Lua"; }
        }

        public static BindType _GT(Type t)
        {
            return new BindType(t);
        }

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            InitGeneralSetting();
            InitBuiltInSearchPathItemList();
            InitWrap();
        }

        public void OnPageBarDraw()
        {
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Generate Wrap")))
            {
                GenerateWrap();
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Clear Wrap")))
            {
                ClearWrap();
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Build")))
            {
                BuildScripts(byteEncode, false);
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Clean Build")))
            {
                BuildScripts(byteEncode, true);
            }
        }

        public void OnSaveDescribe()
        {
            SearchPathItemDescribeSave();
            WrapDescribeSave();
            GeneralSettingSave();

            Config.Serialize();
        }

        // 基础 wrap 类型列表
        private BindType[] baseBindTypes;

        private List<BindType> bindTypes = new List<BindType>();

        private static LuaConfig Config => Core.Serializer<LuaConfig>.Instance;

        private static Preferences_Lua_BuildConfig BuildConfig => Core.Serializer<Preferences_Lua_BuildConfig>.Instance;

        private void CustomRemoveElementFunction_searchPaths(LuaSearchPathItem item)
        {
            searchPaths.Remove(item);
            CheckBuiltInSearchPathItem();
            SearchPathItemDescribeSave();
        }

        #region General Setting

        /// <summary>
        /// 初始化基础设置
        /// </summary>
        private void InitGeneralSetting()
        {
            byteEncode = Config.byteEncode;
        }

        private void GeneralSettingSave()
        {
            Config.byteEncode = byteEncode;
        }

        #endregion General Setting

        #region builtIn Search Path Item

        /// <summary>
        /// 初始化lua搜索路径列表
        /// </summary>
        private void InitBuiltInSearchPathItemList()
        {
            searchPaths.Clear();
            if (Config.searchPaths != null)
            {
                foreach (var t in Config.searchPaths)
                {
                    searchPaths.Add(new LuaSearchPathItem {path = t});
                }
            }

            CheckBuiltInSearchPathItem();
        }

        /// <summary>
        /// 检查内部lua搜索路径
        /// </summary>
        private void CheckBuiltInSearchPathItem()
        {
            var builtIns = new string[]
            {
                "Assets/UFramework/3rd/ToLua/ToLua/Lua",
                "Assets/UFramework/Scripts/Lua",
                "Assets/Scripts/Lua",
                "Assets/Scripts/Automatic/Lua"
            };
            for (var i = 0; i < builtIns.Length; i++)
            {
                var path = builtIns[i];
                foreach (var item in searchPaths)
                {
                    if (item.path.Equals(path))
                    {
                        searchPaths.Remove(item);
                        break;
                    }
                }
            }

            for (var i = 0; i < builtIns.Length; i++)
            {
                searchPaths.Add(new LuaSearchPathItem {path = builtIns[i], order = int.MaxValue - i});
            }

            searchPaths.Sort((left, right) => right.order.CompareTo(left.order));
        }

        /// <summary>
        /// Apply Search PathItem
        /// </summary>
        private void SearchPathItemDescribeSave()
        {
            var count = searchPaths.Count;
            Config.searchPaths = new string[count];
            for (var i = 0; i < count; i++)
            {
                Config.searchPaths[i] = searchPaths[i].path;
            }
        }

        #endregion builtIn Search Path Item

        #region wrap

        /// <summary>
        /// init wrap
        /// </summary>
        private void InitWrap()
        {
            var wrapPath = "Assets/Scripts/Automatic/WrapSource";
            CustomSettings.saveDir = wrapPath + Path.AltDirectorySeparatorChar;
            if (!IOPath.DirectoryExists(wrapPath))
            {
                IOPath.DirectoryCreate(wrapPath);
            }

            if (baseBindTypes == null)
            {
                var bc = CustomSettings.customTypeList.Length;
                baseBindTypes = new BindType[bc];
                for (var i = 0; i < bc; i++)
                {
                    baseBindTypes[i] = CustomSettings.customTypeList[i];
                }
            }

            wrapBindTypes.Clear();
            if (Config.wrapClassNames != null)
            {
                foreach (var t in Config.wrapClassNames)
                {
                    wrapBindTypes.Add( new LuaWrapBindTypeItem {className = t});
                }

                wrapBindTypes.Sort((left, right) => string.Compare(left.className, right.className, StringComparison.Ordinal));
            }
        }

        /// <summary>
        /// Generate Wrap
        /// </summary>
        private void GenerateWrap()
        {
            bindTypes.Clear();
            bindTypes.AddRange(baseBindTypes);
            for (int i = 0; i < wrapBindTypes.Count; i++)
            {
                bindTypes.Add(wrapBindTypes[i].bindType);
            }

            // fairyGUI
            if (Core.Serializer<AppConfig>.Instance.useFairyGUI)
            {
                bindTypes.Add(_GT(typeof(EventContext)));
                bindTypes.Add(_GT(typeof(EventDispatcher)));
                bindTypes.Add(_GT(typeof(EventListener)));
                bindTypes.Add(_GT(typeof(InputEvent)));
                bindTypes.Add(_GT(typeof(DisplayObject)));
                bindTypes.Add(_GT(typeof(Container)));
                bindTypes.Add(_GT(typeof(Stage)));
                bindTypes.Add(_GT(typeof(FairyGUI.Controller)));
                bindTypes.Add(_GT(typeof(GObject)));
                bindTypes.Add(_GT(typeof(GGraph)));
                bindTypes.Add(_GT(typeof(GGroup)));
                bindTypes.Add(_GT(typeof(GImage)));
                bindTypes.Add(_GT(typeof(GLoader)));
                bindTypes.Add(_GT(typeof(GMovieClip)));
                bindTypes.Add(_GT(typeof(TextFormat)));
                bindTypes.Add(_GT(typeof(GTextField)));
                bindTypes.Add(_GT(typeof(GRichTextField)));
                bindTypes.Add(_GT(typeof(GTextInput)));
                bindTypes.Add(_GT(typeof(GComponent)));
                bindTypes.Add(_GT(typeof(GList)));
                bindTypes.Add(_GT(typeof(GRoot)));
                bindTypes.Add(_GT(typeof(GLabel)));
                bindTypes.Add(_GT(typeof(GButton)));
                bindTypes.Add(_GT(typeof(GComboBox)));
                bindTypes.Add(_GT(typeof(GProgressBar)));
                bindTypes.Add(_GT(typeof(GSlider)));
                bindTypes.Add(_GT(typeof(PopupMenu)));
                bindTypes.Add(_GT(typeof(ScrollPane)));
                bindTypes.Add(_GT(typeof(Transition)));
                bindTypes.Add(_GT(typeof(UIPackage)));
                bindTypes.Add(_GT(typeof(Window)));
                bindTypes.Add(_GT(typeof(GObjectPool)));
                bindTypes.Add(_GT(typeof(Relations)));
                bindTypes.Add(_GT(typeof(RelationType)));
                bindTypes.Add(_GT(typeof(GTween)));
                bindTypes.Add(_GT(typeof(GTweener)));
                bindTypes.Add(_GT(typeof(EaseType)));
                bindTypes.Add(_GT(typeof(TweenValue)));
                bindTypes.Add(_GT(typeof(UIObjectFactory)));

                bindTypes.Add(_GT(typeof(UFramework.UI.UIPanel)));
                bindTypes.Add(_GT(typeof(UFramework.UI.FiaryPanel)));
            }

            bindTypes.Add(_GT(typeof(UFramework.UI.Layer)));

            bindTypes.Add(_GT(typeof(UFramework.Network.PackType)));
            bindTypes.Add(_GT(typeof(UFramework.Network.ProtocalType)));
            bindTypes.Add(_GT(typeof(UFramework.Network.ProcessLayer)));
            bindTypes.Add(_GT(typeof(UFramework.Network.SocketChannel)));
            bindTypes.Add(_GT(typeof(UFramework.Network.SocketPack)));
            bindTypes.Add(_GT(typeof(System.Net.Sockets.SocketError)));

            bindTypes.Add(_GT(typeof(UFramework.AppConfig)));
            bindTypes.Add(_GT(typeof(UFramework.UApplication)));
            bindTypes.Add(_GT(typeof(UFramework.IOPath)));
            bindTypes.Add(_GT(typeof(UFramework.Crypt)));

            bindTypes.Add(_GT(typeof(UFramework.ManagerContainer)));
            bindTypes.Add(_GT(typeof(UFramework.NetworkManager)));
            bindTypes.Add(_GT(typeof(UFramework.ResManager)));

            CustomSettings.customTypeList = bindTypes.ToArray();

            ToLuaMenu.GenLuaAll();
        }

        /// <summary>
        /// Clear Wrap
        /// </summary>
        private void ClearWrap()
        {
            string[] files = Directory.GetFiles(CustomSettings.saveDir, "*.cs", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < files.Length; i++)
            {
                File.Delete(files[i]);
            }

            ToLuaExport.Clear();
            List<DelegateType> list = new List<DelegateType>();
            ToLuaExport.GenDelegates(list.ToArray());
            ToLuaExport.Clear();

            StringBuilder sb = new StringBuilder();
            sb.AppendLineEx("using System;");
            sb.AppendLineEx("using LuaInterface;");
            sb.AppendLineEx();
            sb.AppendLineEx("public static class LuaBinder");
            sb.AppendLineEx("{");
            sb.AppendLineEx("\tpublic static void Bind(LuaState L)");
            sb.AppendLineEx("\t{");
            sb.AppendLineEx("\t\tthrow new LuaException(\"Please generate LuaBinder files first!\");");
            sb.AppendLineEx("\t}");
            sb.AppendLineEx("}");

            var file = CustomSettings.saveDir + "LuaBinder.cs";

            using (var textWriter = new StreamWriter(file, false, Encoding.UTF8))
            {
                textWriter.Write(sb.ToString());
                textWriter.Flush();
                textWriter.Close();
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Apply Wrap
        /// </summary>
        private void WrapDescribeSave()
        {
            var count = wrapBindTypes.Count;
            var tbs = new HashSet<string>();
            var num = 0;
            for (var i = 0; i < count; i++)
            {
                var className = wrapBindTypes[i].className;
                if (!tbs.Contains(className))
                {
                    tbs.Add(className);
                    num++;
                }
            }

            Config.wrapClassNames = new string[num];
            foreach (var className in tbs)
            {
                num--;
                Config.wrapClassNames[num] = className;
            }
        }

        #endregion wrap

        #region build code

        /// <summary>
        /// 构建lua代码
        /// </summary>
        public void BuildScripts(bool encode, bool clean)
        {
            var patterns = new string[] {"*.lua"};
            var outPath = IOPath.PathCombine(UApplication.TempDirectory, "Lua");
            if (clean)
            {
                IOPath.DirectoryClear(outPath);
                BuildConfig.files.Clear();
            }

            var fileMap = new Dictionary<string, LuaBuildFile>();
            foreach (var item in BuildConfig.files)
            {
                if (!fileMap.ContainsKey(item.sourcePath))
                    fileMap.Add(item.sourcePath, item);
            }

            var nFileMap = new HashSet<string>();
            var nFiles = new List<LuaBuildFile>();
            for (var i = 0; i < searchPaths.Count; i++)
            {
                var searchItem = searchPaths[i];
                for (var p = 0; p < patterns.Length; p++)
                {
                    var files = IOPath.DirectoryGetFiles(searchItem.path, patterns[p], SearchOption.AllDirectories);
                    for (var k = 0; k < files.Length; k++)
                    {
                        var file = IOPath.PathUnitySeparator(files[k]);

                        var newFile = searchItem.pathMD5 + IOPath.PathReplace(file, searchItem.path);
                        var newFullFile = IOPath.PathCombine(outPath, newFile);

                        var bFile = new LuaBuildFile() {sourcePath = file, destPath = newFile};
                        using (var stream = File.OpenRead(file))
                        {
                            bFile.len = stream.Length;
                            bFile.hash = HashUtils.GetCRC32Hash(stream);
                        }

                        if (!IOPath.FileExists(newFullFile))
                            nFiles.Add(bFile);
                        else if (!fileMap.ContainsKey(file))
                            nFiles.Add(bFile);
                        else
                        {
                            var sourceFile = fileMap[file];
                            if (bFile.len != sourceFile.len || !bFile.hash.Equals(sourceFile.hash))
                                nFiles.Add(bFile);
                        }

                        nFileMap.Add(file);
                    }
                }
            }

            for (var i = 0; i < nFiles.Count; i++)
            {
                var file = nFiles[i];
                var fullFile = IOPath.PathCombine(outPath, file.destPath);
                var dir = Path.GetDirectoryName(fullFile);
                if (!IOPath.DirectoryExists(dir))
                    IOPath.DirectoryCreate(dir);
                var isLuaFile = IOPath.FileExtensionName(file.sourcePath).Equals(".lua");
                if (encode && isLuaFile) EncodeLuaFile(file.sourcePath, fullFile);
                else IOPath.FileCopy(file.sourcePath, fullFile);

                var title = "Processing...[" + i + " - " + nFiles.Count + "]";
                Utils.UpdateProgress(title, file.destPath, i, nFiles.Count);

                if (fileMap.ContainsKey(file.sourcePath))
                    fileMap[file.sourcePath] = file;
                else fileMap.Add(file.sourcePath, file);
            }

            BuildConfig.files.Clear();
            foreach (var item in fileMap)
            {
                var file = item.Value;
                if (nFileMap.Contains(file.sourcePath))
                    BuildConfig.files.Add(file);
                else
                    IOPath.FileDelete(IOPath.PathCombine(outPath, file.destPath));
            }

            BuildConfig.Serialize();

            fileMap.Clear();
            nFileMap.Clear();

            Utils.HideProgress();

            Logger.Debug("build luascript finished. buildFileCount: " + nFiles.Count);
        }

        /// <summary>
        /// encode lua
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="outFile"></param>
        private static void EncodeLuaFile(string srcFile, string outFile)
        {
            srcFile = Path.GetFullPath(srcFile);
            if (!srcFile.ToLower().EndsWith(".lua"))
            {
                File.Copy(srcFile, outFile, true);
                return;
            }

            var isWin = true;
            var luaexe = string.Empty;
            var args = string.Empty;
            var exedir = string.Empty;
            var currDir = Directory.GetCurrentDirectory();
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                isWin = true;
                luaexe = "luajit.exe";
                args = "-b -g " + srcFile + " " + outFile;
                exedir = IOPath.PathCombine(Environment.CurrentDirectory, "LuaEncoder", "luajit");
            }
            else if (Application.platform == RuntimePlatform.OSXEditor)
            {
                isWin = false;
                luaexe = "./luajit";
                args = "-b -g " + srcFile + " " + outFile;
                exedir = IOPath.PathCombine(Environment.CurrentDirectory, "LuaEncoder", "luajit_mac");
            }

            Directory.SetCurrentDirectory(exedir);
            var info = new ProcessStartInfo();
            info.FileName = luaexe;
            info.Arguments = args;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.UseShellExecute = isWin;
            info.ErrorDialog = true;

            Process.Start(info)?.WaitForExit();
            Directory.SetCurrentDirectory(currDir);
        }

        #endregion build code
    }
}