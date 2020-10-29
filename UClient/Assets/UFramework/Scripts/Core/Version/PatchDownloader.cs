/*
 * @Author: fasthro
 * @Date: 2020-10-28 18:08:58
 * @Description: 
 */
using System;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using UFramework.Tools;
using UnityEngine;

namespace UFramework.VersionControl
{
    public enum PatchDownloadStep
    {
        Init,
        Downloading,
        Unzip,
        Completed,
    }

    public class PatchDownloader : IUnzip
    {
        const float BYTES_2_MB = 1f / (1024 * 1024);

        public PatchDownloadStep step { get; private set; }
        public int count { get { return _patchs.Count; } }
        public long downloadSize { get; private set; }
        public float progress { get; private set; }

        private string _baseURL;
        private string _basePath;

        private List<VPatch> _patchs = new List<VPatch>();
        private List<DownloadHandler> _downloads = new List<DownloadHandler>();

        private int _dFinishedCount;
        private int _uFinishedCount;
        private float _baseSp;

        private float _value;
        private float _maxValue;

        private Action _onCompleted;
        private Action _onFailed;

        public PatchDownloader(string baseURL, string basePath, Action onCompleted, Action onFailed)
        {
            _baseURL = baseURL;
            _basePath = basePath;
            _onCompleted = onCompleted;
            _onFailed = onFailed;
            Reset();
        }

        public void Reset()
        {
            downloadSize = 0;
            step = PatchDownloadStep.Init;

            _baseSp = 0;

            _patchs.Clear();
            _downloads.Clear();
        }

        public string GetFormatDownloadSize()
        {
            if (downloadSize >= 1024 * 1024)
                return string.Format("{0:f2}MB", downloadSize * BYTES_2_MB);
            if (downloadSize >= 1024)
                return string.Format("{0:f2}KB", downloadSize / 1024);
            return string.Format("{0:f2}B", downloadSize);
        }

        public void AddPatch(VPatch patch)
        {
            _patchs.Add(patch);
            downloadSize += patch.len;
        }

        public void StartDownload()
        {
            _dFinishedCount = 0;
            _uFinishedCount = 0;
            _baseSp = 1f / (float)count;
            step = PatchDownloadStep.Downloading;
            foreach (var item in _patchs)
                _downloads.Add(Download.AddDownload(GetDownloadURL(item), GetFilePath(item), OnDownloaded, OnDownloadProgress, OnDownloadCanceled, OnDownloadFailed));
        }

        private string GetDownloadURL(VPatch patch)
        {
            return _baseURL + patch.fileName;
        }

        private string GetFilePath(VPatch patch)
        {
            return IOPath.PathCombine(_basePath, patch.fileName);
        }

        private void OnDownloaded()
        {
            _dFinishedCount++;
            if (_dFinishedCount == _downloads.Count) StartUnzip();
        }

        private void OnDownloadProgress(float value)
        {
            progress = _baseSp * (float)_dFinishedCount + value * _baseSp;
        }

        private void OnDownloadFailed(string error) { }
        private void OnDownloadCanceled() { }


        private void StartUnzip()
        {
            step = PatchDownloadStep.Unzip;
            _uFinishedCount = 0;
            _patchs.Sort((x, y) => x.timestamp.CompareTo(y.timestamp));
            Unzip(0);
        }

        private void Unzip(int index)
        {
            var patch = _patchs[index];
            _value = 0;
            _maxValue = patch.files.Length + patch.sFiles.Length;
            UZip.Unzip(GetFilePath(patch), _basePath, null, this);
        }

        public bool OnPreUnzip(ZipEntry entry) { return true; }
        public void OnPostUnzip(ZipEntry entry)
        {
            _value++;

            var pValue = _value / _maxValue;
            progress = _baseSp * (float)_uFinishedCount + pValue * _baseSp;
        }

        public void OnUnzipFinished(bool result)
        {
            _uFinishedCount++;
            if (_uFinishedCount == _patchs.Count)
            {
                step = PatchDownloadStep.Completed;
                _onCompleted.InvokeGracefully();
                
                CleanPatch();
            }
            else Unzip(_uFinishedCount);
        }

        private void CleanPatch()
        {
            foreach (var item in _patchs)
                IOPath.FileDelete(GetFilePath(item));
            Reset();
        }
    }
}