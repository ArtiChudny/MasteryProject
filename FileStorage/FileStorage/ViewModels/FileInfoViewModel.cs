namespace FileStorage.ViewModels
{
    public class FileInfoViewModel
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public long FileSize { get; set; }
        public string CreationDate { get; set; }
        public string Login { get; set; }
        public int DownloadsCount { get; set; }
    }
}
