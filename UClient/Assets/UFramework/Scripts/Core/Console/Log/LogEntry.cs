// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2021/03/02 14:46
// * @Description:
// --------------------------------------------------------------------------------

using System;
using UnityEngine;

namespace UFramework.Consoles
{
    public class LogEntry
    {
        /// <summary>
        /// 日志内容预览长度
        /// </summary>
        private const int LogMessagePreviewLength = 180;

        /// <summary>
        /// 日志堆栈预览长度
        /// </summary>
        private const int LogStackTracePreviewLength = 120;

        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType logType { get; private set; }

        /// <summary>
        /// 日志消息
        /// </summary>
        public string message { get; private set; }

        /// <summary>
        /// 日志预览消息
        /// </summary>
        public string messagePreview { get; private set; }

        /// <summary>
        /// 日志堆栈
        /// </summary>
        public string stackTrace { get; private set; }

        /// <summary>
        /// 日志堆栈预览消息
        /// </summary>
        public string stackTracePreview { get; private set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime dateTime { get; private set; }

        /// <summary>
        /// 相同日志出现次数
        /// </summary>
        public int count { get; private set; }

        public bool isSelected;

        public LogEntry(LogType logType, string message, string stackTrace)
        {
            this.logType = logType;
            this.message = message;
            this.messagePreview = SubPreview(message, LogMessagePreviewLength);
            this.stackTrace = stackTrace;
            this.stackTracePreview = SubPreview(stackTrace, LogStackTracePreviewLength);
            this.count = 1;
            this.dateTime = DateTime.Now;
        }

        public LogEntry(LogEntry entry)
        {
            message = entry.message;
            messagePreview = entry.messagePreview;
            stackTrace = entry.stackTrace;
            stackTracePreview = entry.stackTracePreview;
            logType = entry.logType;
            count = entry.count;
            dateTime = entry.dateTime;
        }

        public void RetainCount()
        {
            count++;
        }

        public void ResetCount()
        {
            count = 1;
        }

        public override string ToString()
        {
            return $"{dateTime:yyyy/MM/dd HH:mm:ss} [{logType.ToString().ToUpper()}] -> {message}{Environment.NewLine}{stackTrace}";
        }

        static string SubPreview(string str, int previewLength)
        {
            var target = str.Split('\n')[0];
            target = target.Substring(0, Mathf.Min(target.Length, previewLength));
            return target;
        }
    }
}