/*
 * @Author: fasthro
 * @Date: 2020-09-15 15:49:23
 * @Description: texture preferences
 */
using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace UFramework.Editor.Preferences.Assets
{
    public class AssetImporterPreferencesPage : IPage, IPageBar
    {
        public string menuName { get { return "AssetImporter/Preferences"; } }
        static AssetImporterSerdata Serdata { get { return Serialize.Serializable<AssetImporterSerdata>.Instance; } }

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
            texturePattern = Serdata.texturePattern;
            modePattern = Serdata.modePattern;
            audioPattern = Serdata.audioPattern;

            textures = Serdata.texturePaths;
            modes = Serdata.modePaths;
            audios = Serdata.audioPaths;

            defaultTextureType = Serdata.defaultTextureType;
            defaultAndroidFormat = Serdata.defaultAndroidFormat;
            defaultIOSFormat = Serdata.defaultIOSFormat;
            defaultTextureMaxSize = Serdata.defaultTextureMaxSize;
        }

        public void OnSaveDescribe()
        {
            if (Serdata == null) return;
            Serdata.texturePaths = textures;
            Serdata.modePaths = modes;
            Serdata.audioPaths = audios;

            Serdata.defaultTextureType = defaultTextureType;
            Serdata.defaultAndroidFormat = defaultAndroidFormat;
            Serdata.defaultIOSFormat = defaultIOSFormat;
            Serdata.defaultTextureMaxSize = defaultTextureMaxSize;

            Serdata.Serialization();
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

            Dictionary<int, int> results = new Dictionary<int, int>();
            int index = 0;
            foreach (var item in Serdata.texturePaths)
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
            else return Serdata.texturePaths[index];
        }
    }
}