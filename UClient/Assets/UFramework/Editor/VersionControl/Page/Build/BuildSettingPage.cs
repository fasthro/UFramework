/*
 * @Author: fasthro
 * @Date: 2020-09-16 18:51:28
 * @Description: build
 */

using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UFramework.Config;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.VersionControl
{
    public class BuildSettingPage : IPage, IPageBar
    {
        public string menuName { get { return "Build/Build Setting"; } }
        static VersionControl_BuildConfig describeObject;

        [ShowInInspector, HideLabel]
        [TabGroup("Android")]
        public BuildSettingPageAndroidTable androidTable = new BuildSettingPageAndroidTable();

        [ShowInInspector, HideLabel]
        [TabGroup("iOS")]
        public BuildSettingPageIOSTable iosTable = new BuildSettingPageIOSTable();

        public object GetInstance()
        {
            return this;
        }

        public void OnRenderBefore()
        {
            describeObject = UConfig.Read<VersionControl_BuildConfig>();

            androidTable.useCustomKeystore = describeObject.useCustomKeystore;
            androidTable.keystoreName = describeObject.keystoreName;
            androidTable.keystorePass = describeObject.keystorePass;
            androidTable.keyaliasName = describeObject.keyaliasName;
            androidTable.keyaliasPass = describeObject.keyaliasPass;

            iosTable.signingTeamID = describeObject.signingTeamID;
        }

        public void OnSaveDescribe()
        {
            describeObject.useCustomKeystore = androidTable.useCustomKeystore;
            describeObject.keystoreName = androidTable.keystoreName;
            describeObject.keystorePass = androidTable.keystorePass;
            describeObject.keyaliasName = androidTable.keyaliasName;
            describeObject.keyaliasPass = androidTable.keyaliasPass;

            describeObject.signingTeamID = iosTable.signingTeamID;

            describeObject.Save();
        }

        public void OnPageBarDraw()
        {

        }
    }
}