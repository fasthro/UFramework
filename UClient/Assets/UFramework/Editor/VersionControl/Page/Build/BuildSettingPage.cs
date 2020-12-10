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

namespace UFramework.Editor.VersionControl
{
    public class BuildSettingPage : IPage, IPageBar
    {
        public string menuName { get { return "Build/Build Setting"; } }
        static VersionBuildSerdata Serdata { get { return Serialize.Serializable<VersionBuildSerdata>.Instance; } }

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
            androidTable.useCustomKeystore = Serdata.useCustomKeystore;
            androidTable.keystoreName = Serdata.keystoreName;
            androidTable.keystorePass = Serdata.keystorePass;
            androidTable.keyaliasName = Serdata.keyaliasName;
            androidTable.keyaliasPass = Serdata.keyaliasPass;

            iosTable.signingTeamID = Serdata.signingTeamID;
        }

        public void OnSaveDescribe()
        {
            Serdata.useCustomKeystore = androidTable.useCustomKeystore;
            Serdata.keystoreName = androidTable.keystoreName;
            Serdata.keystorePass = androidTable.keystorePass;
            Serdata.keyaliasName = androidTable.keyaliasName;
            Serdata.keyaliasPass = androidTable.keyaliasPass;

            Serdata.signingTeamID = iosTable.signingTeamID;

            Serdata.Serialization();
        }

        public void OnPageBarDraw()
        {

        }
    }
}