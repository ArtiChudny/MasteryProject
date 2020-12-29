using FileStorage.BLL.Models.ResponceModels.QueryResponceModels;
using FileStorage.BLL.Queries;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.QueryHandlers
{
    class GetDirectoryInnerListQueryHandler : IRequestHandler<GetDirectoryInnerListQuery, GetDirectoryInnerListResponseModel>
    {
        private readonly IStorageRepository _storageRepository;

        public GetDirectoryInnerListQueryHandler(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }

        async public Task<GetDirectoryInnerListResponseModel> Handle(GetDirectoryInnerListQuery request, CancellationToken cancellationToken)
        {
            var directory = await _storageRepository.GetDirectory(request.Path);
            var responceModel = new GetDirectoryInnerListResponseModel();

            foreach (var dir in directory.Directories)
            {
                responceModel.InnerDirectories.Add(dir.Name);
            }

            foreach (var file in directory.Files)
            {
                responceModel.InnerFiles.Add(file.Name);
            }

            return responceModel;
        }
    }
}
