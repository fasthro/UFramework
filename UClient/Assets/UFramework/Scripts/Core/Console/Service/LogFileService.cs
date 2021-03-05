// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/05 11:20
// * @Description:
// --------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Threading;
using UFramework.Core;
using UnityEngine;
using Console = UFramework.Core.Console;

namespace UFramework.Consoles
{
    public class LogFileService : BaseConsoleService
    {
        private Thread _thread;
        private DoubleQueue<LogEntry> _logQueue;

        private UTF8Encoding _encoding;
        private FileStream _writer;

        private float _intervalTime;
        private float _intervalTimeTemp;
        private bool _canWrite;

        private bool _isFirst;
        private bool _enabled;

        protected override void OnInitialized()
        {
            var consoleConfig = Serializer<ConsoleConfig>.Instance;
            _enabled = consoleConfig.enabledWriteFile;
            
            if (!_enabled)
                return;

            _intervalTime = consoleConfig.writeIntervalTime;

            _logQueue = new DoubleQueue<LogEntry>();

            Console.Instance.logService.logListener += OnLog;

            var now = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
#if UNITY_EDITOR
            var logDir = IOPath.PathCombine(Environment.CurrentDirectory, "Logs");
#else
            var logDir = IOPath.PathCombine(Application.persistentDataPath, "Logs");
#endif
            if (!IOPath.DirectoryExists(logDir))
                IOPath.DirectoryCreate(logDir);

            var fileInfo = new FileInfo(IOPath.PathCombine(logDir, now + ".log"));
            _writer = fileInfo.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

            _encoding = new UTF8Encoding();

            _thread = new Thread(new ThreadStart(OnSave)) {IsBackground = true};
            _thread.Start();
        }

        void OnLog(LogEntry entry)
        {
            if (entry == null)
                return;

            if (!_isFirst)
            {
                _isFirst = true;
                var allEntries = Console.Instance.logService.entries;
                for (var i = 0; i < allEntries.Size; i++)
                    _logQueue.Enqueue(allEntries[i]);
            }
            else
            {
                _logQueue.Enqueue(entry);
            }
        }

        private void OnSave()
        {
            while (true)
            {
                try
                {
                    if (_canWrite)
                    {
                        _canWrite = false;

                        if (_logQueue.IsEmpty())
                            _logQueue.Swap();

                        while (!_logQueue.IsEmpty())
                        {
                            var text = _logQueue.Dequeue().ToString() + Environment.NewLine;
                            _writer.Write(_encoding.GetBytes(text), 0, _encoding.GetByteCount(text));
                            _writer.Flush();
                        }
                    }
                }
                catch (ThreadAbortException e)
                {
                }
            }
        }

        protected override void OnUpdate()
        {
            if (!_canWrite)
            {
                _intervalTimeTemp -= Time.deltaTime;
                if (_intervalTimeTemp <= 0)
                {
                    _intervalTimeTemp = _intervalTime;
                    
                    _logQueue.Swap();
                    if (!_logQueue.IsEmpty())
                    {
                        _canWrite = true;
                    }
                }
            }
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            _thread?.Abort();
            _thread = null;

            _writer.Flush();
            _writer.Close();
            _writer = null;
        }
    }
}