/*
 * @Author: fasthro
 * @Date: 2020-09-29 11:20:22
 * @Description: fiary package
 */
using System.Collections.Generic;
using FairyGUI;
using UFramework.Assets;
using UFramework.Config;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UFramework.UI
{
    public class FairyPackage : Package
    {
        public UIPackage package { get; private set; }

        private AssetRequest m_bundleRequest;

        public FairyPackage(string packageName) : base(packageName) { }

        protected override void LoadMain()
        {
#if UNITY_EDITOR
            package = UIPackage.AddPackage(IOPath.PathCombine(UConfig.Read<AppConfig>().uiDirectory, string.Format("{0}/{1}", packageName, packageName)),
                (string name, string extension, System.Type type, out DestroyMethod destroyMethod) =>
                {
                    destroyMethod = DestroyMethod.Unload;
                    return AssetDatabase.LoadAssetAtPath(name + extension, type);
                }
            );
            LoadDependen();
#else
            m_bundleRequest = Asset.LoadBundleAsync(IOPath.PathCombine(UConfig.Read<AppConfig>().uiDirectory, packageName), (request) =>
            {
                if (!isStandby)
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
                for (int i = 0; i < package.dependencies.Length; i++)
                {
                    foreach (KeyValuePair<string, string> item in package.dependencies[i])
                    {
                        if (item.Key == "name")
                        {
                            if (!dependences.Contains(item.Value) && !packageName.Equals(item.Value))
                            {
                                dependences.Add(item.Value);
                            }
                        }
                    }
                }

                dependenCount = dependences.Count;
                foreach (var item in dependences)
                    PackageAgents.Load(item, OnDependen);
                if (dependenCount == 0) LoadCompleted();
            }
        }

        private void OnDependen()
        {
            dependenCount--;
            if (dependenCount <= 0)
            {
                LoadCompleted();
            }
        }

        protected override void LoadCompleted()
        {
            if (!isStandby)
                LuaManager.Call<string>("fgui.load_component_package", packageName);
            base.LoadCompleted();
        }

        protected override void OnReferenceEmpty()
        {
            if (isStandby) return;

            base.OnReferenceEmpty();

            if (package != null)
                UIPackage.RemovePackage(package.id);
            package = null;

            foreach (var item in dependences)
                PackageAgents.Unload(item);
            dependences.Clear();

            if (m_bundleRequest != null)
                m_bundleRequest.Unload();
            m_bundleRequest = null;
        }
    }
}