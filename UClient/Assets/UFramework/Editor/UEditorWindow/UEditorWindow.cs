/*
 * @Author: fasthro
 * @Date: 2020-06-28 14:01:30
 * @Description: UFramework 编辑器工具
 */
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UFramework.Config;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor
{
    public class UEditorWindow : OdinMenuEditorWindow
    {
        [MenuItem("UFramework/UFramework")]
        private static void OpenWindow()
        {
            var window = GetWindow<UEditorWindow>();
            window.titleContent = new GUIContent("UFramework");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree(true);

            tree.Add("Lua", this);

            tree.Add("AssetBundle/Configure", this);
            tree.Add("AssetBundle/Build", this);
            tree.Add("AssetBundle/Setting", this);

            tree.Add("Pool", this);
            tree.Add("I18N", this);
            tree.Add("Config", new BaseConfig());

            tree.Add("Table", this);
            tree.Add("SDK", this);
            tree.Add("Version", this);
            tree.Add("Setting", this);


            return tree;
        }
    }
}
