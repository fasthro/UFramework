/*
 * @Author: fasthro
 * @Date: 2021-01-05 12:07:14
 * @Description: 
 */
using UnityEditor;
using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Lockstep.Editor
{
    public class SyncScript
    {
        static StringBuilder SB = new StringBuilder();

        static string CP
        {
            get { return Path.Combine(Environment.CurrentDirectory, "Assets/UFramework-Lockstep/Lockstep"); }
        }

        static string SP
        {
            get { return Path.Combine(PathParent(Environment.CurrentDirectory), "LockstepServer/Lockstep/Lockstep"); }
        }

        [MenuItem("Lockstep/同步代码 -> Client To Server")]
        public static void SyncToServer()
        {
            DirectoryCopy(CP, SP, true);
            Debug.Log("sync scripts finished!");
        }

        [MenuItem("Lockstep/同步代码 -> Server To Client")]
        public static void SyncToClient()
        {
            DirectoryCopy(SP, CP, true);
            Debug.Log("sync scripts finished!");
        }

        public static string PathParent(string path, int index = 1)
        {
            if (index < 1)
                return null;

            path = PathUnitySeparator(path);
            string[] ps = path.Split(Path.AltDirectorySeparatorChar);

            if (ps.Length >= index)
            {
                SB.Clear();
                for (int i = 0; i < ps.Length - index; i++)
                {
                    SB.Append(ps[i]);
                    SB.Append(Path.AltDirectorySeparatorChar);
                }
                return SB.ToString();
            }
            return null;
        }

        static string PathUnitySeparator(string path, string separator = null)
        {
            if (string.IsNullOrEmpty(separator))
                separator = Path.AltDirectorySeparatorChar.ToString();
            return path.Replace("\\", separator).Replace("//", separator);
        }

        static void DirectoryCopy(string source, string target, bool clearTarget = true)
        {
            if (clearTarget)
                DirectoryClear(target);

            var files = Directory.GetFiles(source, "*.cs", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < files.Length; i++)
            {
                File.Copy(files[i], files[i].Replace(source, target));
            }

            var dirs = Directory.GetDirectories(source, "*.*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < dirs.Length; i++)
            {
                var dir = dirs[i];
                var newDir = target + dir.Replace(source, "");
                DirectoryCopy(dir, newDir);
            }
        }

        static void DirectoryClear(string directory)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
            Directory.CreateDirectory(directory);
        }
    }
}