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
            var directory = await _storageRepository.GetFullDirectoryTree(request.Path);
            var searchResult = new GetDirectorySearchResultResponseModel();

            FindMatches(directory, request.SearchLine, searchResult);

            return searchResult;
        }

        //method recursively searches for directories and files that match search line, and adds their paths to response model
        private void FindMatches(StorageDirectory directory, string searchLine, GetDirectorySearchResultResponseModel searchResult)
        {
            foreach (var dir in directory.Directories)
            {
                foreach (var file in dir.Files)
                {
                    if (file.Name.Contains(searchLine))
                    {
                        searchResult.MatchedFiles.Add(file.Path);
                    }
                }

                if (dir.Name.Contains(searchLine, StringComparison.InvariantCultureIgnoreCase))
                {
                    searchResult.MatchedDirectories.Add(dir.Path);
                }

                FindMatches(dir, searchLine, searchResult);
            }
        }
    }
}
