/*
 * @Author: fasthro
 * @Date: 2020-09-29 11:20:22
 * @Description: fiary package
 */
using FairyGUI;
using UFramework.Assets;
using UFramework.Config;
using UFramework.Messenger;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UFramework.UI.FairyGUI
{
    public class FairyPackage : Package
    {
        public UIPackage package { get; private set; }

        private AssetRequest m_bundleRequest;

        public FairyPackage(string packageName) : base(packageName) { }

        public override void Load(UCallback onCompleted)
        {
            if (loadState == LoadState.Unloaded) return;

            base.Load(onCompleted);

            if (loadState == LoadState.Loaded)
            {
                OnLoadAsset();
                return;
            }
            else if (loadState == LoadState.Load)
            {
                return;
            }

#if UNITY_EDITOR
            package = UIPackage.AddPackage(IOPath.PathCombine(UConfig.Read<AppConfig>().uiDirectory, string.Format("{0}/{1}", packageName, packageName)),
                (string name, string extension, System.Type type, out DestroyMethod destroyMethod) =>
                {
                    destroyMethod = DestroyMethod.Unload;
                    return AssetDatabase.LoadAssetAtPath(name + extension, type);
                }
            );
            OnLoadAsset();
#else
            m_bundleRequest = Asset.LoadBundleAsync(IOPath.PathCombine(UConfig.Read<AppConfig>().uiDirectory, packageName), (request) =>
            {
                if (!m_isUnloadMark)
                {
                    package = UIPackage.AddPackage(request.asset as AssetBundle);
                }
                OnLoadAsset();
            });
#endif
        }

        protected override void OnReferenceEmpty()
        {
            if (m_remain) return;

            base.OnReferenceEmpty();

            if (package != null)
                UIPackage.RemovePackage(package.id);
            package = null;

            if (m_bundleRequest != null)
                m_bundleRequest.Unload();
            m_bundleRequest = null;
        }
    }
}