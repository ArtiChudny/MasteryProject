using System.Collections.Generic;

namespace FileStorage.BLL.Models.ResponceModels.QueryResponceModels
{
    public class GetDirectoryInnerListResponseModel
    {
        public List<string> InnerDirectories { get; set; }
        public List<string> InnerFiles { get; set; }

        public GetDirectoryInnerListResponseModel()
        {
            InnerDirectories = new List<string>();
            InnerFiles = new List<string>();
        }
    }
}
