/*
 * @Author: fasthro
 * @Date: 2020-08-08 19:39:10
 * @Description: Lua/ToLua Page
 */
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using LuaInterface;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Config;
using UFramework.Lua;
using UFramework.Tools;
using UnityEditor;
using UnityEngine;
using static ToLuaMenu;
using Debug = UnityEngine.Debug;

namespace UFramework.Editor.Preferences
{
    public class LuaPage : IPage, IPageBar
    {
        public string menuName { get { return "Lua"; } }
        static LuaConfig describeObject;
        static AppConfig app;

        [ShowInInspector]
        [BoxGroup("General Setting")]
        [LabelText("Build Use Byte Encode")]
        public bool byteEncode = true;

        /// <summary>
        /// lua 搜索路径
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        [ShowInInspector]
        [ListDrawerSettings(Expanded = true, CustomRemoveElementFunction = "OnSearchPathItemCustomRemoveElementFunction")]
        public List<LuaSearchPathItem> searchPaths = new List<LuaSearchPathItem>();

        /// <summary>
        /// lua bind type items
        /// </summary>
        /// <typeparam name="LuaWrapBindTypeItem"></typeparam>
        /// <returns></returns>
        [ShowInInspector]
        public List<LuaWrapBindTypeItem> wrapBindTypes = new List<LuaWrapBindTypeItem>();

        // 基础 wrap 类型列表
        private BindType[] baseBindTypes;
        private List<BindType> bindTypes = new List<BindType>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            app = UConfig.Read<AppConfig>();
            describeObject = UConfig.Read<LuaConfig>();

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

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Build Scripts")))
            {
                BuildCode(byteEncode);
            }
        }

        public void OnSaveDescribe()
        {
            SearchPathItemDescribeSave();
            WrapDescribeSave();
            GeneralSettingSave();

            describeObject.Save();
        }

        #region General Setting

        /// <summary>
        /// 初始化基础设置
        /// </summary>
        private void InitGeneralSetting()
        {
            byteEncode = describeObject.byteEncode;
        }

        private void GeneralSettingSave()
        {
            describeObject.byteEncode = byteEncode;
        }

        #endregion

        #region builtIn Search Path Item

        /// <summary>
        /// 初始化lua搜索路径列表
        /// </summary>
        private void InitBuiltInSearchPathItemList()
        {
            searchPaths.Clear();
            if (describeObject.searchPaths != null)
            {
                for (int i = 0; i < describeObject.searchPaths.Length; i++)
                {
                    var item = new LuaSearchPathItem();
                    item.path = describeObject.searchPaths[i];
                    searchPaths.Add(item);
                }
            }
            CheckBuiltInSearchPathItem();
        }

        /// <summary>
        /// 检查内部lua搜索路径
        /// </summary>
        private void CheckBuiltInSearchPathItem()
        {
            string[] builtIns = new string[] {
                "Assets/UFramework/3rd/ToLua/ToLua/Lua",
                "Assets/UFramework/Scripts/Lua",
                "Assets/Scripts/Lua",
                "Assets/Scripts/Automatic/Lua" };
            for (int i = 0; i < builtIns.Length; i++)
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

            for (int i = 0; i < builtIns.Length; i++)
            {
                var item = new LuaSearchPathItem();
                item.path = builtIns[i];
                item.order = int.MaxValue - i;
                searchPaths.Add(item);
            }
            searchPaths.Sort((left, right) =>
            {
                return right.order.CompareTo(left.order);
            });
        }

        /// <summary>
        /// Custom Remove Search Path Item Function
        /// </summary>
        /// <param name="item"></param>
        private void OnSearchPathItemCustomRemoveElementFunction(LuaSearchPathItem item)
        {
            searchPaths.Remove(item);
            CheckBuiltInSearchPathItem();
            SearchPathItemDescribeSave();
        }

        /// <summary>
        ///  Apply Search PathItem
        /// </summary>
        private void SearchPathItemDescribeSave()
        {
            var count = searchPaths.Count;
            describeObject.searchPaths = new string[count];
            for (int i = 0; i < count; i++)
            {
                describeObject.searchPaths[i] = searchPaths[i].path;
            }
        }

        #endregion

        #region wrap

        /// <summary>
        /// init wrap
        /// </summary>
        private void InitWrap()
        {
            CustomSettings.saveDir = App.UserScriptAutomaticToLuaDirectory + Path.AltDirectorySeparatorChar;
            if (!IOPath.DirectoryExists(App.UserScriptAutomaticToLuaDirectory))
            {
                IOPath.DirectoryCreate(App.UserScriptAutomaticToLuaDirectory);
            }

            if (baseBindTypes == null)
            {
                var bc = CustomSettings.customTypeList.Length;
                baseBindTypes = new BindType[bc];
                for (int i = 0; i < bc; i++)
                {
                    baseBindTypes[i] = CustomSettings.customTypeList[i];
                }
            }

            wrapBindTypes.Clear();
            if (describeObject.wrapClassNames != null)
            {
                for (int i = 0; i < describeObject.wrapClassNames.Length; i++)
                {
                    var item = new LuaWrapBindTypeItem();
                    item.className = describeObject.wrapClassNames[i];
                    wrapBindTypes.Add(item);
                }

                wrapBindTypes.Sort((left, right) =>
                {
                    return left.className.CompareTo(right.className);
                });
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

            string file = CustomSettings.saveDir + "LuaBinder.cs";

            using (StreamWriter textWriter = new StreamWriter(file, false, Encoding.UTF8))
            {
                textWriter.Write(sb.ToString());
                textWriter.Flush();
                textWriter.Close();
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        ///  Apply Wrap
        /// </summary>
        private void WrapDescribeSave()
        {
            var count = wrapBindTypes.Count;
            HashSet<string> tbs = new HashSet<string>();
            int num = 0;
            for (int i = 0; i < count; i++)
            {
                var className = wrapBindTypes[i].className;
                if (!tbs.Contains(className))
                {
                    tbs.Add(className);
                    num++;
                }
            }

            describeObject.wrapClassNames = new string[num];
            foreach (var className in tbs)
            {
                num--;
                describeObject.wrapClassNames[num] = className;
            }
        }

        #endregion

        #region build code

        /// <summary>
        /// 构建lua代码
        /// </summary>
        private void BuildCode(bool encode)
        {
            IOPath.DirectoryClear(App.LuaTempDataDirectory);
            IOPath.DirectoryClear(App.LuaDataDirectory);

            int n = 0;
            for (int i = 0; i < searchPaths.Count; i++)
            {
                var searchItem = searchPaths[i];
                var files = IOPath.DirectoryGetFiles(searchItem.path);
                n = 0;
                foreach (string file in files)
                {
                    if (file.EndsWith(".meta")) continue;
                    var newFile = IOPath.PathCombine(App.LuaTempDataDirectory, searchItem.pathMD5) + IOPath.PathReplace(file, searchItem.path);
                    var root = Path.GetDirectoryName(newFile);
                    if (!IOPath.DirectoryExists(root)) IOPath.DirectoryCreate(root);
                    if (encode)
                    {
                        EncodeLuaFile(file, newFile);
                    }
                    else
                    {
                        IOPath.FileCopy(file, newFile);
                    }

                    n++;
                    string title = "Processing...[" + n + " - " + files.Length + "]";
                    Utils.UpdateProgress(title, newFile, n, files.Length);
                }
                Utils.HideProgress();
            }
            AssetDatabase.Refresh();

            var zipfile = IOPath.PathCombine(App.DataDirectory, "Scripts.zip");
            IOPath.FileDelete(zipfile);
            IOPath.DirectoryDelete(App.LuaDataDirectory);
            if (app.isDevelopmentVersion)
            {
                IOPath.DirectoryCopy(App.LuaTempDataDirectory, App.LuaDataDirectory);
            }
            else
            {
                UZip.Zip(new string[] { App.LuaTempDataDirectory }, zipfile);
            }
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        /// <summary>
        /// encode lua
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="outFile"></param>
        static void EncodeLuaFile(string srcFile, string outFile)
        {
            srcFile = Path.GetFullPath(srcFile);
            if (!srcFile.ToLower().EndsWith(".lua"))
            {
                File.Copy(srcFile, outFile, true);
                return;
            }
            bool isWin = true;
            string luaexe = string.Empty;
            string args = string.Empty;
            string exedir = string.Empty;
            string currDir = Directory.GetCurrentDirectory();
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                isWin = true;
                luaexe = "luajit.exe";
                args = "-b -g " + srcFile + " " + outFile;
                exedir = IOPath.PathCombine(IOPath.PathParent(Application.dataPath), "LuaEncoder", "luajit");
            }
            else if (Application.platform == RuntimePlatform.OSXEditor)
            {
                isWin = false;
                luaexe = "./luajit";
                args = "-b -g " + srcFile + " " + outFile;
                exedir = IOPath.PathCombine(IOPath.PathParent(Application.dataPath), "LuaEncoder", "luajit_mac");
            }
            Directory.SetCurrentDirectory(exedir);
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = luaexe;
            info.Arguments = args;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.UseShellExecute = isWin;
            info.ErrorDialog = true;
            Debug.Log(info.FileName + " " + info.Arguments);

            Process pro = Process.Start(info);
            pro.WaitForExit();
            Directory.SetCurrentDirectory(currDir);
        }
        #endregion
    }
}