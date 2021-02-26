// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-09-15 15:49:23
// * @Description:
// --------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace UFramework.Editor.Preferences.AssetImporter
{
    public class AssetImporterPage : IPage, IPageBar
    {
        public string menuName => "AssetImporter";

        static Preferences_AssetImporter_Config Config => Core.Serializer<Preferences_AssetImporter_Config>.Instance;

        [ShowInInspector] [TableList(IsReadOnly = true, AlwaysExpanded = true)] [TabGroup("Texture")] [LabelText("Textures")]
        public List<TextureItem> textures = new List<TextureItem>();

        [ShowInInspector] [TableList(IsReadOnly = true, AlwaysExpanded = true)] [TabGroup("Model")] [LabelText("Models")]
        public List<TextureItem> modes = new List<TextureItem>();

        [ShowInInspector] [TableList(IsReadOnly = true, AlwaysExpanded = true)] [TabGroup("Audio")] [LabelText("Audios")] [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        public List<TextureItem> audios = new List<TextureItem>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            textures = Config.textures;
            modes = Config.modes;
            audios = Config.audios;

            if (textures.Count <= 0 || modes.Count <= 0)
            {
                AnalysisSearchPaths();
            }
        }

        public void OnSaveDescribe()
        {
            if (Config == null) return;
            Config.textures = textures;
            Config.modes = modes;
            Config.audios = audios;
            Config.Serialize();
        }

        public void OnPageBarDraw()
        {
            if (SirenixEditorGUI.ToolbarButton("Reimport Texture"))
            {
                ReimportTexture();
            }

            if (SirenixEditorGUI.ToolbarButton("Reimport Mode"))
            {
            }

            if (SirenixEditorGUI.ToolbarButton("Reimport Audio"))
            {
            }

            if (SirenixEditorGUI.ToolbarButton("Reimport All"))
            {
            }

            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
            {
                AnalysisSearchPaths();
            }
        }

        /// <summary>
        /// analysis search paths
        /// </summary>
        private void AnalysisSearchPaths()
        {
            // texture
            textures.Clear();
            for (var i = 0; i < Config.texturePaths.Count; i++)
            {
                var texture = Config.texturePaths[i];
                var files = ParsePath(texture.path, Config.texturePattern);
                for (var k = 0; k < files.Length; k++)
                {
                    var file = files[k];
                    var item = new TextureItem();
                    item.path = IOPath.PathUnitySeparator(file);
                    item.textureType = texture.textureType;
                    item.androidFormat = texture.androidFormat;
                    item.iosFormat = texture.iosFormat;
                    item.maxSize = texture.maxSize;
                    item.Update();
                    textures.Add(item);
                }
            }
        }

        /// <summary>
        /// parse path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        static string[] ParsePath(string path, string pattern)
        {
            var items = new List<string>();
            var patterns = pattern.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in patterns)
            {
                var files = Directory.GetFiles(path, item, SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    if (!ValidateAssetPath(file)) continue;
                    items.Add(file);
                }
            }

            return items.ToArray();
        }

        /// <summary>
        /// validate
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static bool ValidateAssetPath(string path)
        {
            if (!path.StartsWith("Assets/")) return false;

            var ext = "*" + Path.GetExtension(path).ToLower();

            bool legal = false;

            var patterns = Config.texturePattern.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in patterns)
            {
                if (item.Equals(ext))
                {
                    legal = true;
                    break;
                }
            }

            if (!legal)
            {
                patterns = Config.modePattern.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in patterns)
                {
                    if (item.Equals(ext))
                    {
                        legal = true;
                        break;
                    }
                }
            }

            if (!legal)
            {
                patterns = Config.audioPattern.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in patterns)
                {
                    if (item.Equals(ext))
                    {
                        legal = true;
                        break;
                    }
                }
            }

            return legal;
        }

        /// <summary>
        /// Reimport Texture
        /// </summary>
        private void ReimportTexture()
        {
            var n = 0;
            foreach (var texture in textures)
            {
                TexturePreImporter.Execute(texture);

                Utils.UpdateProgress("Reimport texture", texture.path, n++, textures.Count);
            }

            Utils.HideProgress();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Reimport Texture
        /// </summary>
        /// <param name="searchItem"></param>
        public static void ReimportTexture(TextureSearchPathItem searchItem)
        {
            var files = ParsePath(searchItem.path, Config.texturePattern);
            for (var k = 0; k < files.Length; k++)
            {
                var file = files[k];
                var item = new TextureItem();
                item.path = IOPath.PathUnitySeparator(file);
                item.textureType = searchItem.textureType;
                item.androidFormat = searchItem.androidFormat;
                item.iosFormat = searchItem.iosFormat;
                item.maxSize = searchItem.maxSize;

                TexturePreImporter.Execute(item);

                Utils.UpdateProgress("Reimport texture", item.path, k, files.Length);
            }

            Utils.HideProgress();
            AssetDatabase.Refresh();
        }
    }
}