// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-09-15 15:49:23
// * @Description:
// --------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Preferences.AssetImporter
{
    public class AssetImporterPreferencesPage : IPage, IPageBar
    {
        public string menuName => "AssetImporter/Preferences";
        static Preferences_AssetImporter_Config Config => Core.Serializer<Preferences_AssetImporter_Config>.Instance;

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
            texturePattern = Config.texturePattern;
            modePattern = Config.modePattern;
            audioPattern = Config.audioPattern;

            textures = Config.texturePaths;
            modes = Config.modePaths;
            audios = Config.audioPaths;

            defaultTextureType = Config.defaultTextureType;
            defaultAndroidFormat = Config.defaultAndroidFormat;
            defaultIOSFormat = Config.defaultIOSFormat;
            defaultTextureMaxSize = Config.defaultTextureMaxSize;
        }

        public void OnSaveDescribe()
        {
            if (Config == null) return;
            Config.texturePaths = textures;
            Config.modePaths = modes;
            Config.audioPaths = audios;

            Config.defaultTextureType = defaultTextureType;
            Config.defaultAndroidFormat = defaultAndroidFormat;
            Config.defaultIOSFormat = defaultIOSFormat;
            Config.defaultTextureMaxSize = defaultTextureMaxSize;

            Config.Serialize();
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
            var parts = path.Split(Path.AltDirectorySeparatorChar);

            var results = new Dictionary<int, int>();
            var index = 0;
            foreach (var item in Config.texturePaths)
            {
                var cparts = item.path.Split(Path.AltDirectorySeparatorChar);
                var count = Mathf.Max(parts.Length, cparts.Length);
                for (var i = 0; i < count; i++)
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
            var num = -1;
            foreach (var item in results)
            {
                if (item.Value > num)
                {
                    num = item.Value;
                    index = item.Key;
                }
            }

            return index == -1 ? null : Config.texturePaths[index];
        }
    }
}