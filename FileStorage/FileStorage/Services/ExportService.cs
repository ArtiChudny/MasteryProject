using FileStorage.Models;
using System.Xml.Serialization;
using System.IO;
using System.Text.Json;

namespace FileStorage.Services
{
    public class ExportService
    {
        public void ExportFileToXML(StorageInfo storageInfo, string filePath)
        {
            using (FileStream fileStream = new FileStream("filePath", FileMode.OpenOrCreate))
            {
                XmlSerializer formatter = new XmlSerializer(typeof(StorageInfo));
                formatter.Serialize(fileStream, storageInfo);
            }
        }

        public void ExportFileToJSON(StorageInfo storageInfo, string filePath)
        {
            using (FileStream fileStream = new FileStream("filePath", FileMode.OpenOrCreate))
            {
                JsonSerializer.SerializeAsync(fileStream, storageInfo);
            }
        }
    }
}
