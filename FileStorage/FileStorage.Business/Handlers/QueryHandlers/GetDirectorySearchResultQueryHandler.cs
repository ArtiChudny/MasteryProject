using FileStorage.BLL.Models.ResponceModels.QueryResponceModels;
using FileStorage.BLL.Queries;
using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.QueryHandlers
{
    class GetDirectorySearchResultQueryHandler : IRequestHandler<GetDirectorySearchResultQuery, GetDirectorySearchResultResponseModel>
    {
        private readonly IStorageRepository _storageRepository;

        public GetDirectorySearchResultQueryHandler(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        public async Task<GetDirectorySearchResultResponseModel> Handle(GetDirectorySearchResultQuery request, CancellationToken cancellationToken)
        {
            var directory = await _storageRepository.GetDirectory(request.Path);
            var searchResult = new GetDirectorySearchResultResponseModel();
            var pathWithoutCurrentDirectory = request.Path.Substring(0, request.Path.Length - directory.Name.Length - 1);

            Search(searchResult, request.SearchLine, directory, pathWithoutCurrentDirectory);

            return searchResult;
        }

        private void Search(GetDirectorySearchResultResponseModel searchResult, string searchLine, StorageDirectory directory, string path)
        {
            path += $"/{directory.Name}";
            foreach (var dir in directory.Directories)
            {
                foreach (var file in dir.Value.Files)
                {
                    if (file.Key.Contains(searchLine))
                    {
                        searchResult.MatchedFiles.Add($"{path}/{file.Key}");
                    }
                }

                if (dir.Key.ToLower().Contains(searchLine.ToLower()))
                {
                    searchResult.MatchedDirectories.Add($"{path}/{dir.Key}");
                }
                Search(searchResult, searchLine, dir.Value, path);
            }
        }
    }
}
