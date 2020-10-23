/*
 * @Author: fasthro
 * @Date: 2020-10-23 12:05:09
 * @Description: MenuItems
 */
using UFramework.Config;
using UnityEditor;
using UnityEngine;

namespace UFramework.Editor
{
    public static class MenuItems
    {
        const string Tools = "UFramework/Tools/";

        [MenuItem(Tools + "Cleanup", false, 2000)]
        static void Cleanup()
        {
            // Config
            IOPath.DirectoryClear(IOPath.PathCombine(App.AssetsDirectory, "Config", FileAddress.Editor.ToString()));
            IOPath.DirectoryClear(IOPath.PathCombine(App.AssetsDirectory, "Config", FileAddress.Resources.ToString()));
            IOPath.DirectoryClear(IOPath.PathCombine(App.AssetsDirectory, "Config", FileAddress.Data.ToString()));
        }
    }
}