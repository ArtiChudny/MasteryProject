using FileStorage.BLL.Models.ResponceModels.QueryResponceModels;
using FileStorage.BLL.Queries;
using FileStorage.DAL;
using FileStorage.DAL.Constants;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.QueryHandlers
{
    public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, UserInfoResponseModel>
    {
        private readonly IStorageRepository _storageRepository;
        private readonly CurrentUser _currentUser;

        public GetUserInfoQueryHandler(IStorageRepository storageRepository, CurrentUser currentUser)
        {
            _storageRepository = storageRepository;
            _currentUser = currentUser;
        }

        public async Task<UserInfoResponseModel> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            var usedStorage = await _storageRepository.GetUsedStorage(DirectoryPaths.InitialDirectoryPath);
            var directory = await _storageRepository.GetDirectory(DirectoryPaths.InitialDirectoryPath);
            
            var userInfo = new UserInfoResponseModel
            {
                Login = _currentUser.Login,
                UsedStorage = usedStorage,
                CreationDate = directory.CreationDate
            };

            return userInfo;
        }
    }
}
