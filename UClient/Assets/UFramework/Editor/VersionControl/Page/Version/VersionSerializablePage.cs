/*
 * @Author: fasthro
 * @Date: 2020-09-16 18:51:28
 * @Description: version
 */

using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Assets;
using UFramework.Config;
using UFramework.VersionControl;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.VersionControl
{
    public class VersionSerializablePage : IPage, IPageBar
    {
        public string menuName { get { return "Version/Serializable"; } }

        [FilePath]
        public string versionFilePath;

        [Button]
        public void Serializable()
        {
            version = Version.LoadVersion(versionFilePath);
        }

        [ShowInInspector]
        public Version version;

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {

        }

        public void OnSaveDescribe()
        {

        }

        public void OnPageBarDraw()
        {

        }
    }
}