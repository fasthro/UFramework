/*
 * @Author: fasthro
 * @Date: 2020-09-29 11:20:22
 * @Description: fiary resource package
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
    public class FairyResourcesPackage : Package
    {
        public UIPackage package { get; private set; }

        public FairyResourcesPackage(string packageName) : base(packageName) { }

        protected override void LoadMain()
        {
            package = UIPackage.AddPackage(string.Format("UI/{0}/{1}", packageName, packageName));
            LoadDependen();
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
                    PackageAgents.LoadFairyResource(item, OnDependen);
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
        }
    }
}