// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020-09-29 11:20:22
// * @Description:
// --------------------------------------------------------------------------------

using FairyGUI;
using UFramework.Core;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UFramework.UI
{
    public class FairyPackage : Package
    {
        public UIPackage package { get; private set; }

        private AssetRequest _bundleRequest;

        public FairyPackage(string packageName) : base(packageName)
        {
        }

        protected override void LoadMain()
        {
#if UNITY_EDITOR
            package = UIPackage.AddPackage(IOPath.PathCombine(Serializer<AppConfig>.Instance.uiDirectory, $"{packageName}/{packageName}"),
                (string name, string extension, System.Type type, out DestroyMethod destroyMethod) =>
                {
                    destroyMethod = DestroyMethod.Unload;
                    return AssetDatabase.LoadAssetAtPath(name + extension, type);
                }
            );
            LoadDependen();
#else
            var bundleName = Assets.BundlePath2BundleName(IOPath.PathCombine(Serializer<AppConfig>.Instance.uiDirectory, packageName));
            // Debug.Log($"fairy pack load mian: {packageName}>{bundleName}");
            _bundleRequest = Assets.LoadBundleAsync(bundleName, (request) =>
            {
                // Debug.Log($"fairy pack loaded mian: {request.name}");
                if (!_isStandby)
                {
                    package = UIPackage.AddPackage(request.asset as AssetBundle);
                    LoadDependen();
                }
                else LoadCompleted();
            });
#endif
        }

        protected override void LoadDependen()
        {
            if (package != null)
            {
                foreach (var t in package.dependencies)
                {
                    foreach (var item in t)
                    {
                        if (item.Key == "name" && !item.Value.Equals("_Font"))
                        {
                            if (!_dependences.Contains(item.Value) && !packageName.Equals(item.Value))
                            {
                                _dependences.Add(item.Value);
                            }
                        }
                    }
                }

                _dependenCount = _dependences.Count;
                foreach (var item in _dependences)
                    PackageAgents.Load(item, OnDependen);
                if (_dependenCount == 0) LoadCompleted();
            }
        }

        private void OnDependen()
        {
            _dependenCount--;
            if (_dependenCount <= 0)
            {
                LoadCompleted();
            }
        }

        protected override void LoadCompleted()
        {
            if (!_isStandby)
                LuaManager.Call<string>("fgui.load_component_package", packageName);
            base.LoadCompleted();
        }

        protected override void OnReferenceEmpty()
        {
            if (_isStandby) return;

            base.OnReferenceEmpty();

            if (package != null)
                UIPackage.RemovePackage(package.id);
            package = null;

            foreach (var item in _dependences)
                PackageAgents.Unload(item);
            _dependences.Clear();

            _bundleRequest?.Unload();
            _bundleRequest = null;
        }
    }
}