/*
 * @Author: fasthro
 * @Date: 2020-09-16 18:51:28
 * @Description: build
 */

using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor.VersionControl.Build
{
    public class BuildSettingPage : IPage, IPageBar
    {
        public string menuName { get { return "Build/Build Setting"; } }
        static VersionControl_Build_Config Config { get { return Core.Serializer<VersionControl_Build_Config>.Instance; } }

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
            androidTable.useCustomKeystore = Config.useCustomKeystore;
            androidTable.keystoreName = Config.keystoreName;
            androidTable.keystorePass = Config.keystorePass;
            androidTable.keyaliasName = Config.keyaliasName;
            androidTable.keyaliasPass = Config.keyaliasPass;

            iosTable.signingTeamID = Config.signingTeamID;
        }

        public void OnSaveDescribe()
        {
            Config.useCustomKeystore = androidTable.useCustomKeystore;
            Config.keystoreName = androidTable.keystoreName;
            Config.keystorePass = androidTable.keystorePass;
            Config.keyaliasName = androidTable.keyaliasName;
            Config.keyaliasPass = androidTable.keyaliasPass;

            Config.signingTeamID = iosTable.signingTeamID;

            Config.Serialize();
        }

        public void OnPageBarDraw()
        {

        }
    }
}