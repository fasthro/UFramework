/*
 * @Author: fasthro
 * @Date: 2020-09-15 16:03:29
 * @Description: 
 */
using UnityEngine;
using UnityEditor;

namespace UFramework.Editor.Preferences.AssetImporter
{
    public class TexturePreImporter
    {
        static Preferences_AssetImporter_Config Config { get { return Core.Serializer<Preferences_AssetImporter_Config>.Instance; } }

        public static void Execute(TextureItem item)
        {
            var importer = (TextureImporter)TextureImporter.GetAtPath(item.path);
            importer.mipmapEnabled = false;
            importer.compressionQuality = 50;
            switch (item.textureType)
            {
                case TextureType.Default:
                    importer.textureType = TextureImporterType.Default;
                    break;
                case TextureType.Sprite:
                    importer.textureType = TextureImporterType.Sprite;
                    break;
                case TextureType.SpriteAtlas:
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = SpriteImportMode.Multiple;
                    break;
                case TextureType.FairyAtlas:
                    importer.textureType = TextureImporterType.Default;
                    importer.textureShape = TextureImporterShape.Texture2D;
                    importer.filterMode = FilterMode.Bilinear;
                    break;
                case TextureType.NormalMap:
                    importer.textureType = TextureImporterType.NormalMap;
                    break;
            }

            var andSettings = new TextureImporterPlatformSettings();
            andSettings.name = "Android";
            andSettings.overridden = true;
            andSettings.compressionQuality = importer.compressionQuality;
            andSettings.format = item.androidFormat;
            andSettings.maxTextureSize = item.GetTextureSize();
            importer.SetPlatformTextureSettings(andSettings);

            var iosSettings = new TextureImporterPlatformSettings();
            iosSettings.name = "iPhone";
            iosSettings.overridden = true;
            iosSettings.compressionQuality = importer.compressionQuality;
            iosSettings.format = item.iosFormat;
            iosSettings.maxTextureSize = item.GetTextureSize();

            importer.SetPlatformTextureSettings(iosSettings);
        }

        public static void Preprocess(string path, TextureImporter importer)
        {
            var searchItem = AssetImporterPreferencesPage.ContainsTextureSearchPath(path);
            TextureItem texure = new TextureItem();
            if (searchItem == null)
            {
                texure.path = path;
                texure.textureType = Config.defaultTextureType;
                texure.androidFormat = Config.defaultAndroidFormat;
                texure.iosFormat = Config.defaultIOSFormat;
                texure.maxSize = Config.defaultTextureMaxSize;
            }
            else
            {
                texure.path = path;
                texure.textureType = searchItem.textureType;
                texure.androidFormat = searchItem.androidFormat;
                texure.iosFormat = searchItem.iosFormat;
                texure.maxSize = searchItem.maxSize;
            }
            Execute(texure);
        }
    }
}