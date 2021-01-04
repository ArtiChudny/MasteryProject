using FileStorage.BLL.Models.ResponceModels.QueryResponceModels;
using FileStorage.BLL.Queries;
using FileStorage.DAL;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.QueryHandlers
{
    public class GetDirectoryInfoQueryHandler : IRequestHandler<GetDirectoryInfoQuery, DirectoryInfoResponseModel>
    {
        private readonly IStorageRepository _storageRepository;
        private readonly CurrentUser _currentUser;

        public GetDirectoryInfoQueryHandler(IStorageRepository storageRepository, CurrentUser currentUser)
        {
            _storageRepository = storageRepository;
            _currentUser = currentUser;
        }

        public async Task<DirectoryInfoResponseModel> Handle(GetDirectoryInfoQuery request, CancellationToken cancellationToken)
        {
            var directory = await _storageRepository.GetDirectory(request.Path);
            long directorySize = await _storageRepository.GetUsedStorage(directory.Path);

            var responseModel = new DirectoryInfoResponseModel()
            {
                CreationDate = directory.CreationDate,
                ModificationDate = directory.ModificationDate,
                Name = directory.Name,
                Path = directory.Path,
                Size = directorySize,
                Login = _currentUser.Login
            };

            return responseModel;
        } 
    }
}
