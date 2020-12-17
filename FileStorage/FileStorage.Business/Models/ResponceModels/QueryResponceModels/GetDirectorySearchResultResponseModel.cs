using System.Collections.Generic;

namespace FileStorage.BLL.Models.ResponceModels.QueryResponceModels
{
    public class GetDirectorySearchResultResponseModel
    {
        public List<string> MatchedDirectories { get; set; }
        public List<string> MatchedFiles { get; set; }

        public GetDirectorySearchResultResponseModel()
        {
            MatchedDirectories = new List<string>();
            MatchedFiles = new List<string>();
        }
    }
}
