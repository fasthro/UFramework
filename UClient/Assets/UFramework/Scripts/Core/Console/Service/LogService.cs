// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/02 14:27
// * @Description:
// --------------------------------------------------------------------------------

using System;
using UFramework.Core;
using UnityEngine;

namespace UFramework.Consoles
{
    public class LogService : BaseConsoleService
    {
        public delegate void RefreshEvent();

        /// <summary>
        /// error count
        /// </summary>
        public int errorCount { get; private set; }

        /// <summary>
        /// warning count
        /// </summary>
        public int warningCount { get; private set; }

        /// <summary>
        /// debug count
        /// </summary>
        public int debugCount { get; private set; }
        
        /// <summary>
        /// 日志
        /// </summary>
        public CircularBuffer<LogEntry> entries => _hasCleared ? _entries : _allEntries;

        private CircularBuffer<LogEntry> _allEntries;
        private CircularBuffer<LogEntry> _entries;

        /// <summary>
        /// 刷新监听
        /// </summary>
        public event RefreshEvent refreshListener;


        private bool _collapseEnabled;
        private bool _hasCleared;

        private readonly object _lockObject = new object();


        protected override void OnInitialized()
        {
            _collapseEnabled = true;
            _allEntries = new CircularBuffer<LogEntry>(2048);
            Application.logMessageReceivedThreaded += LogMessageReceivedThreaded;
        }

        public void Clear()
        {
            lock (_lockObject)
            {
                _hasCleared = true;

                if (_entries == null)
                {
                    _entries = new CircularBuffer<LogEntry>(2048);
                }
                else
                {
                    _entries.Clear();
                }

                errorCount = warningCount = debugCount = 0;
            }

            refreshListener?.Invoke();
        }

        private void LogMessageReceivedThreaded(string condition, string stackTrace, LogType type)
        {
            lock (_lockObject)
            {
                var prevMessage = _collapseEnabled && _allEntries.Size > 0 ? _allEntries[_allEntries.Size - 1] : null;

                SettCounter(type, 1);

                if (prevMessage != null && prevMessage.logType == type && prevMessage.message == condition && prevMessage.stackTrace == stackTrace)
                {
                    DuplicatedEntry(prevMessage);
                }
                else
                {
                    AddEntry(new LogEntry(type, condition, stackTrace));
                }
            }
        }

        private void AddEntry(LogEntry entry)
        {
            if (_hasCleared)
            {
                if (_entries.IsFull)
                {
                    SettCounter(_entries.Front().logType, -1);
                    _entries.PopFront();
                }

                _entries.PushBack(entry);
            }
            else
            {
                if (_allEntries.IsFull)
                {
                    SettCounter(_allEntries.Front().logType, -1);
                    _allEntries.PopFront();
                }
            }

            _allEntries.PushBack(entry);

            refreshListener?.Invoke();
        }

        private void DuplicatedEntry(LogEntry entry)
        {
            entry.RetainCount();

            refreshListener?.Invoke();

            if (_hasCleared && _entries.IsEmpty)
            {
                var newEntry = new LogEntry(entry);
                newEntry.ResetCount();
                AddEntry(newEntry);
            }
        }

        private void SettCounter(LogType type, int amount)
        {
            switch (type)
            {
                case LogType.Assert:
                case LogType.Error:
                case LogType.Exception:
                    errorCount += amount;
                    break;

                case LogType.Warning:
                    warningCount += amount;
                    break;

                case LogType.Log:
                    debugCount += amount;
                    break;
            }
        }
    }
}