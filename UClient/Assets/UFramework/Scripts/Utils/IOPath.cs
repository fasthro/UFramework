/*
 * @Author: fasthro
 * @Date: 2020-05-30 17:22:39
 * @Description: file, path tools
 */

using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace UFramework
{
    public static class IOPath
    {
        // 字符串操作使用
        static StringBuilder SB = new StringBuilder();

        #region Directory

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="directory"></param>
        public static void DirectoryCreate(string directory)
        {
            try
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }
            catch { }
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="recursive"></param>
        public static void DirectoryDelete(string directory, bool recursive = true)
        {
            try
            {
                if (Directory.Exists(directory))
                    Directory.Delete(directory, recursive);
            }
            catch { }
        }

        /// <summary>
        /// 清理目录
        /// </summary>
        /// <param name="directory"></param>
        public static void DirectoryClear(string directory)
        {
            DirectoryDelete(directory);
            DirectoryCreate(directory);
        }

        /// <summary>
        /// 目录是否存在
        /// </summary>
        /// <param name="directory"></param>
        public static bool DirectoryExists(string directory)
        {
            return Directory.Exists(directory);
        }

        /// <summary>
        /// Copy 目录
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="clearTarget">是否清理目标目录</param>
        public static void DirectoryCopy(string source, string target, bool clearTarget = true)
        {
            if (clearTarget)
                DirectoryClear(target);

            var files = Directory.GetFiles(source, "*.*", SearchOption.TopDirectoryOnly);
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

        /// <summary>
        /// GetFiles 目录
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="searchPattern"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static string[] DirectoryGetFiles(string directory, string searchPattern = "*.*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            if (!Directory.Exists(directory))
                return new string[0];
            return Directory.GetFiles(directory, searchPattern, searchOption);
        }

        #endregion

        #region Path
        /// <summary>
        /// 合并路径
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <param name="isUnified"></param>
        /// <returns></returns>
        public static string PathCombine(string path1, string path2, bool isUnified = true)
        {
            return isUnified ? PathUnitySeparator(Path.Combine(path1, path2)) : Path.Combine(path1, path2);
        }

        /// <summary>
        /// 合并路径
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <param name="path3"></param>
        /// <param name="isUnified"></param>
        /// <returns></returns>
        public static string PathCombine(string path1, string path2, string path3, bool isUnified = true)
        {
            return isUnified ? PathUnitySeparator(Path.Combine(path1, path2, path3)) : Path.Combine(path1, path2, path3);
        }

        /// <summary>
        /// 合并路径
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <param name="path3"></param>
        /// <param name="path4"></param>
        /// <param name="isUnified"></param>
        /// <returns></returns>
        public static string PathCombine(string path1, string path2, string path3, string path4, bool isUnified = true)
        {
            return isUnified ? PathUnitySeparator(Path.Combine(path1, path2, path3, path4)) : Path.Combine(path1, path2, path3, path4);
        }

        /// <summary>
        /// 合并路径
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string PathCombine(params string[] paths)
        {
            return Path.Combine(paths);
        }

        /// <summary>
        /// 切割路径,并且返回所选名称
        /// </summary>
        /// <param name="index">index (< 0 or > 0)</param>
        /// <returns></returns>
        public static string PathSection(string path, int index)
        {
            if (index == 0)
                return null;

            path = PathUnitySeparator(path);
            string[] ps = path.Split(Path.AltDirectorySeparatorChar);

            if (index < 0)
                index = ps.Length + index + 1;

            if (ps.Length >= index)
                return ps[index - 1];

            return null;
        }

        /// <summary>
        /// 统一路径分隔符
        /// </summary>
        /// <param name="path"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string PathUnitySeparator(string path, string separator = null)
        {
            if (string.IsNullOrEmpty(separator))
                separator = Path.AltDirectorySeparatorChar.ToString();
            return path.Replace("\\", separator).Replace("//", separator);
        }

        /// <summary>
        /// 获取上级目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="index"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Replace Path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string PathReplace(string path, string oldValue, string newValue = "")
        {
            return path.Replace(oldValue, newValue);
        }


        /// <summary>
        /// Reset Relatative To Assets Path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string PathRelativeAsset(string path)
        {
            return path.Replace(PathParent(Application.dataPath), "");
        }

        #endregion

        #region File

        /// <summary>
        /// 创建文本文件（UTF8）
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool FileCreateText(string path, string content)
        {
            return FileCreateText(path, content, Encoding.UTF8);
        }

        /// <summary>
        /// 创建文本文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static bool FileCreateText(string path, string content, Encoding encoding)
        {
            try
            {
                FileInfo info = new FileInfo(path);

                if (!info.Directory.Exists)
                    info.Directory.Create();

                if (info.Exists)
                    info.Delete();

                if (encoding == null) File.WriteAllText(info.FullName, content);
                else File.WriteAllText(info.FullName, content, encoding);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        /// 
        /// /// <summary>
        /// 读取文本文件（UTF8）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FileReadText(string path)
        {
            return FileReadText(path, Encoding.UTF8);
        }

        /// <summary>
        /// 读取文本文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string FileReadText(string path, Encoding encoding)
        {
            try
            {
                FileInfo info = new FileInfo(path);
                if (info.Exists)
                    return File.ReadAllText(info.FullName);
            }
            catch (Exception e)
            {
            }
            return null;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FileDelete(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception e)
            {
            }
            return true;
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Copy 文件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool FileCopy(string source, string dest)
        {
            try
            {
                FileInfo info = new FileInfo(dest);

                if (!Directory.Exists(info.DirectoryName))
                    Directory.CreateDirectory(info.DirectoryName);

                FileDelete(dest);

                File.Copy(source, dest);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="source">目标文件路径</param>
        /// <param name="newName">新文件名称</param>
        public static void FileRename(string source, string newName)
        {
            FileInfo info = new FileInfo(source);
            string np = IOPath.PathCombine(info.Directory.FullName, newName);
            FileDelete(np);
            File.Move(source, np);
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static long FileSize(string path)
        {
            return (new FileInfo(path)).Length;
        }

        /// <summary>
        /// 文件 md5
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string FileMD5(string path)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        /// <param name="path"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string FileName(string path, bool extension = false)
        {
            return extension ? Path.GetFileName(path) : Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// 文件后缀名称
        /// </summary>
        /// <param name="path"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string FileExtensionName(string path)
        {
            return Path.GetExtension(path);
        }
        #endregion
    }
}