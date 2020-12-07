using System;
using System.Diagnostics;
using System.Threading;

namespace FileStorage.ConsoleUI.Helpers
{
    public static class ConvertingHelper
    {
        public static string GetDateString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        public static string GetSizeString(long size)
        {
            if (size < 1024)
            {
                return size.ToString();
            }
            else if (size < 1048576)
            {
                return GetKbString(size);
            }
            else if (size < 1073741824)
            {
                return GetMbString(size);
            }
            else
            {
                return GetGbString(size);
            }
        }

        public static string GetKbString(long size)
        {
            var kbSize = Math.Round((double)size / 1024, 2);

            return $"{kbSize}KB";
        }

        private static string GetMbString(long size)
        {
            var mbSize = Math.Round((double)size / 1024 / 1024, 2);

            return $"{mbSize}MB";
        }

        private static string GetGbString(long size)
        {
            var gbSize = Math.Round((double)size / 1024 / 1024 / 1024, 2);

            return $"{gbSize}GB";
        }

        public static string GetLogMessage(string message, string stackTrace)
        {
            int processId = Process.GetCurrentProcess().Id;
            int threadId = Thread.CurrentThread.ManagedThreadId;

            return $"{message}; ProcessId:{processId} ; ThreadId:{threadId}; {stackTrace}";
        }
    }
}
