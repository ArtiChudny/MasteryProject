﻿using System;

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
            double MbSize = Math.Round((double)size / 1024, 2);

            return $"{MbSize}KB";
        }

        public static string GetMbString(long size)
        {
            double MbSize = Math.Round((double)size / 1024 / 1024, 2);

            return $"{MbSize}MB";
        }

        public static string GetGbString(long size)
        {
            double MbSize = Math.Round((double)size / 1024 / 1024 / 1024, 2);

            return $"{MbSize}GB";
        }
    }
}
