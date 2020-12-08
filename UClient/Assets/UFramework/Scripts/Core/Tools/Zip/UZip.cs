/*
 * @Author: fasthro
 * @Date: 2020-05-21 23:29:56
 * @Description: zip
 */ 


using System.IO;
using UnityEngine;
using ICSharpCode.SharpZipLib.Zip;

namespace UFramework.Tools
{
    public static class UZip
    {
        /// <summary>
        /// 压缩文件，指定每个文件目录
        /// </summary>
        /// <param name="fileArray"></param>
        /// <param name="parentbArray"></param>
        /// <param name="_outputPathName"></param>
        /// <param name="_password"></param>
        /// <param name="_zipCallback"></param>
        /// <returns></returns>
        public static bool Zip(string[] fileArray, string[] parentbArray, string _outputPathName, string _password = null, IZip _zipCallback = null)
        {
            if ((null == fileArray) || null == parentbArray || fileArray.Length != fileArray.Length || string.IsNullOrEmpty(_outputPathName))
            {
                if (null != _zipCallback)
                    _zipCallback.OnZipFinished(false);

                return false;
            }

            ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(_outputPathName));
            zipOutputStream.SetLevel(6);    // 压缩质量和压缩速度的平衡点
            if (!string.IsNullOrEmpty(_password))
                zipOutputStream.Password = _password;

            for (int index = 0; index < fileArray.Length; ++index)
            {
                bool result = false;
                string fileOrDirectory = fileArray[index];
                if (File.Exists(fileOrDirectory))
                    result = ZipFile(fileOrDirectory, parentbArray[index], zipOutputStream, _zipCallback);

                if (!result)
                {
                    if (null != _zipCallback)
                        _zipCallback.OnZipFinished(false);

                    return false;
                }
            }

            zipOutputStream.Finish();
            zipOutputStream.Close();

            if (null != _zipCallback)
                _zipCallback.OnZipFinished(true);

            return true;
        }

        /// <summary>
        /// 压缩文件和文件夹
        /// </summary>
        /// <param name="_fileOrDirectoryArray">文件夹路径和文件名</param>
        /// <param name="_outputPathName">压缩后的输出路径文件名</param>
        /// <param name="_password">压缩密码</param>
        /// <param name="_zipCallback">ZipCallback对象，负责回调</param>
        /// <returns></returns>
        public static bool Zip(string[] _fileOrDirectoryArray, string _outputPathName, string _password = null, IZip _zipCallback = null)
        {
            if ((null == _fileOrDirectoryArray) || string.IsNullOrEmpty(_outputPathName))
            {
                if (null != _zipCallback)
                    _zipCallback.OnZipFinished(false);

                return false;
            }

            ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(_outputPathName));
            zipOutputStream.SetLevel(6);    // 压缩质量和压缩速度的平衡点
            if (!string.IsNullOrEmpty(_password))
                zipOutputStream.Password = _password;

            for (int index = 0; index < _fileOrDirectoryArray.Length; ++index)
            {
                bool result = false;
                string fileOrDirectory = _fileOrDirectoryArray[index];
                if (Directory.Exists(fileOrDirectory))
                    result = ZipDirectory(fileOrDirectory, string.Empty, zipOutputStream, _zipCallback);
                else if (File.Exists(fileOrDirectory))
                    result = ZipFile(fileOrDirectory, string.Empty, zipOutputStream, _zipCallback);

                if (!result)
                {
                    if (null != _zipCallback)
                        _zipCallback.OnZipFinished(false);

                    return false;
                }
            }

            zipOutputStream.Finish();
            zipOutputStream.Close();

            if (null != _zipCallback)
                _zipCallback.OnZipFinished(true);

            return true;
        }

        /// <summary>
        /// 解压Zip包
        /// </summary>
        /// <param name="_filePathName">Zip包的文件路径名</param>
        /// <param name="_outputPath">解压输出路径</param>
        /// <param name="_password">解压密码</param>
        /// <param name="_unzipCallback">UnzipCallback对象，负责回调</param>
        /// <returns></returns>
        public static bool Unzip(string _filePathName, string _outputPath, string _password = null, IUnzip _unzipCallback = null)
        {
            if (string.IsNullOrEmpty(_filePathName) || string.IsNullOrEmpty(_outputPath))
            {
                if (null != _unzipCallback)
                    _unzipCallback.OnUnzipFinished(false);

                return false;
            }

            try
            {
                return Unzip(File.OpenRead(_filePathName), _outputPath, _password, _unzipCallback);
            }
            catch (System.Exception _e)
            {
                Logger.Error("[ZipUtility.UnzipFile]: " + _e.ToString());

                if (null != _unzipCallback)
                    _unzipCallback.OnUnzipFinished(false);

                return false;
            }
        }

        /// <summary>
        /// 解压Zip包
        /// </summary>
        /// <param name="_fileBytes">Zip包字节数组</param>
        /// <param name="_outputPath">解压输出路径</param>
        /// <param name="_password">解压密码</param>
        /// <param name="_unzipCallback">UnzipCallback对象，负责回调</param>
        /// <returns></returns>
        public static bool Unzip(byte[] _fileBytes, string _outputPath, string _password = null, IUnzip _unzipCallback = null)
        {
            if ((null == _fileBytes) || string.IsNullOrEmpty(_outputPath))
            {
                if (null != _unzipCallback)
                    _unzipCallback.OnUnzipFinished(false);

                return false;
            }

            bool result = Unzip(new MemoryStream(_fileBytes), _outputPath, _password, _unzipCallback);
            if (!result)
            {
                if (null != _unzipCallback)
                    _unzipCallback.OnUnzipFinished(false);
            }

            return result;
        }

        /// <summary>
        /// 解压Zip包
        /// </summary>
        /// <param name="_inputStream">Zip包输入流</param>
        /// <param name="_outputPath">解压输出路径</param>
        /// <param name="_password">解压密码</param>
        /// <param name="_unzipCallback">UnzipCallback对象，负责回调</param>
        /// <returns></returns>
        public static bool Unzip(Stream _inputStream, string _outputPath, string _password = null, IUnzip _unzipCallback = null)
        {
            if ((null == _inputStream) || string.IsNullOrEmpty(_outputPath))
            {
                if (null != _unzipCallback)
                    _unzipCallback.OnUnzipFinished(false);

                return false;
            }

            // 创建文件目录
            if (!Directory.Exists(_outputPath))
                Directory.CreateDirectory(_outputPath);

            // 解压Zip包
            ZipEntry entry = null;
            using (ZipInputStream zipInputStream = new ZipInputStream(_inputStream))
            {
                if (!string.IsNullOrEmpty(_password))
                    zipInputStream.Password = _password;

                while (null != (entry = zipInputStream.GetNextEntry()))
                {
                    if (string.IsNullOrEmpty(entry.Name))
                        continue;

                    if ((null != _unzipCallback) && !_unzipCallback.OnPreUnzip(entry))
                        continue;   // 过滤

                    string filePath = IOPath.PathUnitySeparator(Path.Combine(_outputPath, entry.Name));

                    // 创建文件目录
                    if (entry.IsDirectory)
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                        continue;
                    }

                    // 写入文件
                    try
                    {
                        using (FileStream fileStream = File.Create(filePath))
                        {
                            byte[] bytes = new byte[1024];
                            while (true)
                            {
                                int count = zipInputStream.Read(bytes, 0, bytes.Length);
                                if (count > 0)
                                    fileStream.Write(bytes, 0, count);
                                else
                                {
                                    if (null != _unzipCallback)
                                        _unzipCallback.OnPostUnzip(entry);

                                    break;
                                }
                            }
                        }
                    }
                    catch (System.Exception _e)
                    {
                        Logger.Error("[ZipUtility.UnzipFile]: " + _e.ToString());

                        if (null != _unzipCallback)
                            _unzipCallback.OnUnzipFinished(false);

                        return false;
                    }
                }
            }

            if (null != _unzipCallback)
                _unzipCallback.OnUnzipFinished(true);

            return true;
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="_filePathName">文件路径名</param>
        /// <param name="_parentRelPath">要压缩的文件的父相对文件夹</param>
        /// <param name="_zipOutputStream">压缩输出流</param>
        /// <param name="_zipCallback">ZipCallback对象，负责回调</param>
        /// <returns></returns>
        private static bool ZipFile(string _filePathName, string _parentRelPath, ZipOutputStream _zipOutputStream, IZip _zipCallback = null)
        {
            //Crc32 crc32 = new Crc32();
            ZipEntry entry = null;
            FileStream fileStream = null;
            try
            {
                string entryName = _parentRelPath + '/' + Path.GetFileName(_filePathName);
                entry = new ZipEntry(entryName);
                entry.DateTime = System.DateTime.Now;

                if ((null != _zipCallback) && !_zipCallback.OnPreZip(entry))
                    return true;    // 过滤

                fileStream = File.OpenRead(_filePathName);
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);
                fileStream.Close();

                entry.Size = buffer.Length;

                //crc32.Reset();
                //crc32.Update(buffer);
                //entry.Crc = crc32.Value;

                _zipOutputStream.PutNextEntry(entry);
                _zipOutputStream.Write(buffer, 0, buffer.Length);
            }
            catch (System.Exception _e)
            {
                Logger.Error("[ZipUtility.ZipFile]: Failled File: " + _filePathName);
                Logger.Error("[ZipUtility.ZipFile]: " + _e.ToString());
                return false;
            }
            finally
            {
                if (null != fileStream)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }

            if (null != _zipCallback)
                _zipCallback.OnPostZip(entry);

            return true;
        }

        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="_path">要压缩的文件夹</param>
        /// <param name="_parentRelPath">要压缩的文件夹的父相对文件夹</param>
        /// <param name="_zipOutputStream">压缩输出流</param>
        /// <param name="_zipCallback">ZipCallback对象，负责回调</param>
        /// <returns></returns>
        private static bool ZipDirectory(string _path, string _parentRelPath, ZipOutputStream _zipOutputStream, IZip _zipCallback = null)
        {
            ZipEntry entry = null;
            try
            {
                string entryName = Path.Combine(_parentRelPath, Path.GetFileName(_path) + '/');
                entry = new ZipEntry(entryName);
                entry.DateTime = System.DateTime.Now;
                entry.Size = 0;

                if ((null != _zipCallback) && !_zipCallback.OnPreZip(entry))
                    return true;    // 过滤

                _zipOutputStream.PutNextEntry(entry);
                _zipOutputStream.Flush();

                string[] files = Directory.GetFiles(_path);
                for (int index = 0; index < files.Length; ++index)
                    ZipFile(files[index], Path.Combine(_parentRelPath, Path.GetFileName(_path)), _zipOutputStream, _zipCallback);
            }
            catch (System.Exception _e)
            {
                Logger.Error("[ZipUtility.ZipDirectory]: " + _e.ToString());
                return false;
            }

            string[] directories = Directory.GetDirectories(_path);
            for (int index = 0; index < directories.Length; ++index)
            {
                if (!ZipDirectory(directories[index], Path.Combine(_parentRelPath, Path.GetFileName(_path)), _zipOutputStream, _zipCallback))
                    return false;
            }

            if (null != _zipCallback)
                _zipCallback.OnPostZip(entry);

            return true;
        }
    }
}