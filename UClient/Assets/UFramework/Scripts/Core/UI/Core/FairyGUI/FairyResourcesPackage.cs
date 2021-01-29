/*
 * @Author: fasthro
 * @Date: 2020-09-29 11:20:22
 * @Description: fiary resource package
 */
using System.Collections.Generic;
using FairyGUI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UFramework.UI
{
    public class FairyResourcesPackage : Package
    {
        public UIPackage package { get; private set; }

        public FairyResourcesPackage(string packageName) : base(packageName)
        {

        }

        protected override void LoadMain()
        {
            package = UIPackage.AddPackage(string.Format("UI/{0}/{1}", packageName, packageName));
            LoadDependen();
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
                    PackageAgents.LoadFairyResource(item, OnDependen);
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
        }
    }
}