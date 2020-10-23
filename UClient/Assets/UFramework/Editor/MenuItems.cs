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

        [MenuItem(Tools + "Cleanup -> Project", false, 2000)]
        static void Cleanup()
        {
            // Config
            IOPath.DirectoryClear(IOPath.PathCombine(App.AssetsDirectory, "Config", FileAddress.Editor.ToString()));
            IOPath.DirectoryClear(IOPath.PathCombine(App.AssetsDirectory, "Config", FileAddress.Resources.ToString()));
            IOPath.DirectoryClear(IOPath.PathCombine(App.AssetsDirectory, "Config", FileAddress.Data.ToString()));
        }

        [MenuItem(Tools + "Cleanup -> UTemp Path", false, 2001)]
        static void CleanupUTempPath()
        {
            IOPath.DirectoryClear(App.TempDirectory);
        }

        [MenuItem(Tools + "Cleanup -> Persistent Data Path", false, 2002)]
        static void CleanupPersistentDataPath()
        {
            IOPath.DirectoryClear(Application.persistentDataPath);
        }

        [MenuItem(Tools + "Open -> Persistent Data Path", false, 2003)]
        static void OpenPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
    }
}