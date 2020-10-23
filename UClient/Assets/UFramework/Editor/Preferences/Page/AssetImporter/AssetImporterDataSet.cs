/*
 * @Author: fasthro
 * @Date: 2020-09-15 16:15:16
 * @Description: 
 */

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UFramework.Config;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace UFramework.Editor.Preferences.Assets
{
    /// <summary>
    /// texture max size
    /// </summary>
    public enum TextureMaxSize
    {
        MaxSize_4096,
        MaxSize_2048,
        MaxSize_1024,
        MaxSize_512,
        MaxSize_256,
        MaxSize_128,
        MaxSize_64,
        MaxSize_32,
    }

    public enum TextureType
    {
        Default = 0,
        Sprite = 8,
        SpriteAtlas = 100,
        FairyAtlas = 101,
        NormalMap = 1,
    }

    /// <summary>
    /// texture search path item
    /// </summary>
    [System.Serializable]
    public class TextureSearchPathItem
    {
        /// <summary>
        /// 路径
        /// </summary>
        [ShowInInspector, HideLabel, FolderPath]
        [HorizontalGroup("Search Path")]
        public string path;

        /// <summary>
        /// Texture Type
        /// </summary>
        [ShowInInspector, HideLabel]
        [HorizontalGroup("Texture Type")]
        [TableColumnWidth(100, false)]
        public TextureType textureType = TextureType.Default;

        /// <summary>
        /// Android Format
        /// </summary>
        [ShowInInspector, HideLabel]
        [HorizontalGroup("Android Format")]
        [TableColumnWidth(120, false)]
        public TextureImporterFormat androidFormat = TextureImporterFormat.ETC2_RGBA8;

        /// <summary>
        /// iOS Format
        /// </summary>
        [ShowInInspector, HideLabel]
        [HorizontalGroup("iOS Format")]
        [TableColumnWidth(120, false)]
        public TextureImporterFormat iosFormat = TextureImporterFormat.ASTC_6x6;

        /// <summary>
        /// Max Size
        /// </summary>
        [ShowInInspector, HideLabel]
        [HorizontalGroup("Max Size")]
        [TableColumnWidth(120, false)]
        public TextureMaxSize maxSize = TextureMaxSize.MaxSize_1024;

        /// <summary>
        /// 重新导入
        /// </summary>
        [Button]
        [HorizontalGroup("Operation")]
        [LabelText("Reimport")]
        [TableColumnWidth(80, false)]
        public void Reimport() { AssetImporterPage.ReimportTexture(this); }

        /// <summary>
        /// TextureMaxSize to int
        /// </summary>
        /// <returns></returns>
        public int GetTextureSize()
        {
            switch (maxSize)
            {
                case TextureMaxSize.MaxSize_4096: return 4096;
                case TextureMaxSize.MaxSize_2048: return 2048;
                case TextureMaxSize.MaxSize_1024: return 1024;
                case TextureMaxSize.MaxSize_512: return 512;
                case TextureMaxSize.MaxSize_256: return 256;
                case TextureMaxSize.MaxSize_128: return 128;
                case TextureMaxSize.MaxSize_64: return 64;
                case TextureMaxSize.MaxSize_32: return 32;
            }
            return 1024;
        }
    }

    /// <summary>
    /// texture search path item
    /// </summary>
    [System.Serializable]
    public class TextureItem
    {
        /// <summary>
        /// 路径
        /// </summary>
        [ShowInInspector, HideLabel, ReadOnly]
        [HorizontalGroup("Path")]
        public string path;

        /// <summary>
        /// target
        /// </summary>
        [ShowInInspector, HideLabel]
        [HorizontalGroup("Target")]
        [TableColumnWidth(100, false)]
        private UnityEngine.Object target;

        /// <summary>
        /// Texture Type
        /// </summary>
        [ShowInInspector, HideLabel, ReadOnly]
        [HorizontalGroup("Texture Type")]
        [TableColumnWidth(100, false)]
        public TextureType textureType = TextureType.Default;

        /// <summary>
        /// Android Format
        /// </summary>
        [ShowInInspector, HideLabel, ReadOnly]
        [HorizontalGroup("Android Format")]
        [TableColumnWidth(120, false)]
        public TextureImporterFormat androidFormat = TextureImporterFormat.ETC2_RGBA8;

        /// <summary>
        /// iOS Format
        /// </summary>
        [ShowInInspector, HideLabel, ReadOnly]
        [HorizontalGroup("iOS Format")]
        [TableColumnWidth(120, false)]
        public TextureImporterFormat iosFormat = TextureImporterFormat.ASTC_6x6;

        /// <summary>
        /// Max Size
        /// </summary>
        [ShowInInspector, HideLabel, ReadOnly]
        [HorizontalGroup("Texture Max Size")]
        [TableColumnWidth(120, false)]
        public TextureMaxSize maxSize = TextureMaxSize.MaxSize_1024;

        /// <summary>
        /// Size
        /// </summary>
        [ShowInInspector, HideLabel, ReadOnly]
        [HorizontalGroup("File Size")]
        [TableColumnWidth(70, false)]
        public string fileSizeStr
        {
            get
            {
                if (fileSize == 0)
                {
                    return "--";
                }
                return EditorUtility.FormatBytes(fileSize);
            }
        }

        [HideInInspector]
        public long fileSize;

        /// <summary>
        /// 内存占用大小
        /// </summary>
        [ShowInInspector, HideLabel, ReadOnly]
        [HorizontalGroup("Memory")]
        [TableColumnWidth(70, false)]
        public string memorySizeStr
        {
            get
            {
                if (memorySize == 0)
                {
                    return "--";
                }
                return EditorUtility.FormatBytes(memorySize);
            }
        }

        [HideInInspector]
        public long memorySize;

        /// <summary>
        /// TextureMaxSize to int
        /// </summary>
        /// <returns></returns>
        public int GetTextureSize()
        {
            switch (maxSize)
            {
                case TextureMaxSize.MaxSize_4096: return 4096;
                case TextureMaxSize.MaxSize_2048: return 2048;
                case TextureMaxSize.MaxSize_1024: return 1024;
                case TextureMaxSize.MaxSize_512: return 512;
                case TextureMaxSize.MaxSize_256: return 256;
                case TextureMaxSize.MaxSize_128: return 128;
                case TextureMaxSize.MaxSize_64: return 64;
                case TextureMaxSize.MaxSize_32: return 32;
            }
            return 1024;
        }

        /// <summary>
        /// update
        /// </summary>
        public void Update()
        {
            target = AssetDatabase.LoadAssetAtPath<Texture>(path);
            fileSize = IOPath.FileSize(path);
            memorySize = Profiler.GetRuntimeMemorySizeLong(target as Texture);
        }
    }

    /// <summary>
    /// config
    /// </summary>
    public class AssetImporterConfig : IConfigObject
    {
        public FileAddress address { get { return FileAddress.Editor; } }

        public string texturePattern = "*.png,*.tga";
        public string modePattern = "*.fbx";
        public string audioPattern = "*.mp3,*.wav,*.ogg";

        public TextureType defaultTextureType = TextureType.Default;
        public TextureImporterFormat defaultAndroidFormat = TextureImporterFormat.ETC2_RGBA8;
        public TextureImporterFormat defaultIOSFormat = TextureImporterFormat.ASTC_6x6;
        public TextureMaxSize defaultTextureMaxSize = TextureMaxSize.MaxSize_1024;

        public List<TextureSearchPathItem> texturePaths = new List<TextureSearchPathItem>();
        public List<TextureSearchPathItem> modePaths = new List<TextureSearchPathItem>();
        public List<TextureSearchPathItem> audioPaths = new List<TextureSearchPathItem>();

        public List<TextureItem> textures = new List<TextureItem>();
        public List<TextureItem> modes = new List<TextureItem>();
        public List<TextureItem> audios = new List<TextureItem>();

        public void Save()
        {
            UConfig.Write<AssetImporterConfig>(this);
        }
    }
}