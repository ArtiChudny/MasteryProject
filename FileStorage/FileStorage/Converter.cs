using System;

namespace FileStorage
{
    public class Converter
    {
        public string GetDateString(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        public string GetSizeString(long size)
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

        private string GetKbString(long size)
        {
            double MbSize = Math.Round((double)size / 1024, 2);

            return $"{MbSize}KB";
        }

        private string GetMbString(long size)
        {
            double MbSize = Math.Round((double)size / 1024 / 1024, 2);

            return $"{MbSize}MB";
        }

        private string GetGbString(long size)
        {
            double MbSize = Math.Round((double)size / 1024 / 1024 / 1024, 2);

            return $"{MbSize}GB";
        }
    }
}
