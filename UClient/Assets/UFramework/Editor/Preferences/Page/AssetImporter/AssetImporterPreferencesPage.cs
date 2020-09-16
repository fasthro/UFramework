/*
 * @Author: fasthro
 * @Date: 2020-09-15 15:49:23
 * @Description: texture preferences
 */
using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UFramework.Config;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace UFramework.Editor.Preferences.Assets
{
    public class AssetImporterPreferencesPage : IPage, IPageBar
    {
        public string menuName { get { return "AssetImporter/Preferences"; } }
        static AssetImporterConfig describeObject;

        /// <summary>
        /// texture pattern
        /// </summary>
        [BoxGroup("Pattern")]
        [LabelText("Texture")]
        public string texturePattern;

        /// <summary>
        /// mode pattern
        /// </summary>
        [BoxGroup("Pattern")]
        [LabelText("Mode")]
        public string modePattern;

        /// <summary>
        /// audio pattern
        /// </summary>
        [BoxGroup("Pattern")]
        [LabelText("Audio")]
        public string audioPattern;


        [BoxGroup("Default")]
        [LabelText("Texture Type")]
        public TextureType defaultTextureType = TextureType.Default;

        [BoxGroup("Default")]
        [LabelText("Android Format")]
        public TextureImporterFormat defaultAndroidFormat = TextureImporterFormat.ETC2_RGBA8;

        [BoxGroup("Default")]
        [LabelText("iOS Format")]
        public TextureImporterFormat defaultIOSFormat = TextureImporterFormat.ASTC_6x6;

        [BoxGroup("Default")]
        [LabelText("Texture Max Size")]
        public TextureMaxSize defaultTextureMaxSize = TextureMaxSize.MaxSize_1024;


        [ShowInInspector]
        [TableList(AlwaysExpanded = true)]
        [TabGroup("Texture")]
        [LabelText("Texture Search Paths")]
        public List<TextureSearchPathItem> textures = new List<TextureSearchPathItem>();

        [ShowInInspector]
        [TableList(AlwaysExpanded = true)]
        [TabGroup("Model")]
        [LabelText("Model Search Paths")]
        public List<TextureSearchPathItem> modes = new List<TextureSearchPathItem>();

        [ShowInInspector]
        [TableList(AlwaysExpanded = true)]
        [TabGroup("Audio")]
        [LabelText("Audio Search Paths")]
        public List<TextureSearchPathItem> audios = new List<TextureSearchPathItem>();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            describeObject = UConfig.Read<AssetImporterConfig>();

            texturePattern = describeObject.texturePattern;
            modePattern = describeObject.modePattern;
            audioPattern = describeObject.audioPattern;

            textures = describeObject.texturePaths;
            modes = describeObject.modePaths;
            audios = describeObject.audioPaths;

            defaultTextureType = describeObject.defaultTextureType;
            defaultAndroidFormat = describeObject.defaultAndroidFormat;
            defaultIOSFormat = describeObject.defaultIOSFormat;
            defaultTextureMaxSize = describeObject.defaultTextureMaxSize;
        }

        public void OnSaveDescribe()
        {
            describeObject.texturePaths = textures;
            describeObject.modePaths = modes;
            describeObject.audioPaths = audios;

            describeObject.defaultTextureType = defaultTextureType;
            describeObject.defaultAndroidFormat = defaultAndroidFormat;
            describeObject.defaultIOSFormat = defaultIOSFormat;
            describeObject.defaultTextureMaxSize = defaultTextureMaxSize;

            describeObject.Save();
        }

        public void OnPageBarDraw()
        {
        }

        /// <summary>
        /// 路径是否包含在配置中
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static TextureSearchPathItem ContainsTextureSearchPath(string path)
        {
            path = IOPath.PathUnitySeparator(path);
            string[] parts = path.Split(Path.AltDirectorySeparatorChar);

            List<TextureSearchPathItem> searchPaths = new List<TextureSearchPathItem>();
            if (describeObject == null)
            {
                describeObject = UConfig.Read<AssetImporterConfig>();
            }
            searchPaths = describeObject.texturePaths;

            Dictionary<int, int> results = new Dictionary<int, int>();
            int index = 0;
            foreach (var item in searchPaths)
            {
                string[] cparts = item.path.Split(Path.AltDirectorySeparatorChar);
                var count = Mathf.Max(parts.Length, cparts.Length);
                for (int i = 0; i < count; i++)
                {
                    if (i < parts.Length && i < cparts.Length)
                    {
                        if (parts[i].Equals(cparts[i]))
                        {
                            if (!results.ContainsKey(index))
                            {
                                results.Add(index, i);
                            }
                            else
                            {
                                results[index] = i;
                            }
                        }
                    }
                }
                index++;
            }

            index = -1;
            int num = -1;
            foreach (KeyValuePair<int, int> item in results)
            {
                if (item.Value > num)
                {
                    num = item.Value;
                    index = item.Key;
                }
            }

            if (index == -1) return null;
            else return searchPaths[index];
        }
    }
}