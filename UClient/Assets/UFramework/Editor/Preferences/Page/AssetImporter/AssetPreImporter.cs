// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-09-15 16:08:02
// * @Description:
// --------------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.Preferences.AssetImporter
{
    public class AssetPreImporter : AssetPostprocessor
    {
        public void OnPreprocessTexture()
        {
            TexturePreImporter.Preprocess(this.assetPath, (TextureImporter) this.assetImporter);
        }
    }
}