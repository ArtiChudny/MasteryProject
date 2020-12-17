using FileStorage.BLL.Models.ResponceModels.QueryResponceModels;
using FileStorage.BLL.Queries;
using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System;
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
            var pathWithoutCurrentDirectory = request.Path.Remove(request.Path.IndexOf(directory.Name) - 1, directory.Name.Length + 1);

            FindMatches(directory, request.SearchLine, pathWithoutCurrentDirectory, searchResult);

            return searchResult;
        }

        //method recursively searches for directories and files that match search line, and adds their paths to response model
        private void FindMatches(StorageDirectory directory, string searchLine, string path, GetDirectorySearchResultResponseModel searchResult)
        {
            path += $"/{directory.Name}";
            foreach (var dir in directory.Directories)
            {
                foreach (var file in dir.Value.Files)
                {
                    if (file.Key.Contains(searchLine))
                    {
                        searchResult.MatchedFiles.Add($"{path}/{dir.Key}/{file.Key}");
                    }
                }

                if (dir.Key.Contains(searchLine, StringComparison.InvariantCultureIgnoreCase))
                {
                    searchResult.MatchedDirectories.Add($"{path}/{dir.Key}");
                }

                FindMatches(dir.Value, searchLine, path, searchResult);
            }
        }
    }
}
