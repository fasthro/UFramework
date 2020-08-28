/*
 * @Author: fasthro
 * @Date: 2020-08-08 19:39:10
 * @Description: Lua/ToLua Page
 */
using System.Collections.Generic;
using System.IO;
using System.Text;
using LuaInterface;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Config;
using UFramework.Lua;
using UnityEditor;
using UnityEngine;
using static ToLuaMenu;

namespace UFramework.Editor.Preferences
{
    public class LuaPage : IPage, IPageBar
    {
        public string menuName { get { return "Lua"; } }
        static LuaConfig describeObject;

        /// <summary>
        /// lua 搜索路径
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        [ShowInInspector]
        [ListDrawerSettings(CustomRemoveElementFunction = "OnSearchPathItemCustomRemoveElementFunction")]
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
            describeObject = UConfig.Read<LuaConfig>();

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
        }

        public void OnSaveDescribe()
        {
            SearchPathItemDescribeSave();
            WrapDescribeSave();
            describeObject.Save();
        }

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
            string[] builtIns = new string[] { "Assets/UFramework/3rd/ToLua/ToLua/Lua",
            "Assets/UFramework/Scripts/Lua",
            "Assets/UFramework/Scripts/Lua/Core",
            "Assets/UFramework/Scripts/Lua/Core/TypeSystem",
            "Assets/Scripts/Lua" };
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
    }
}