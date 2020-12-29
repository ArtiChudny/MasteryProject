using FileStorage.BLL.Models.ResponceModels.QueryResponceModels;
using FileStorage.BLL.Queries;
using FileStorage.DAL.Constants;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.QueryHandlers
{
    public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, UserInfoResponseModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IStorageRepository _storageRepository;

        public GetUserInfoQueryHandler(IUserRepository userProvider, IStorageRepository storageProvider)
        {
            _userRepository = userProvider;
            _storageRepository = storageProvider;
        }

        public async Task<UserInfoResponseModel> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _userRepository.GetUser();
            var usedStorage = await _storageRepository.GetUsedStorage(DirectoryPaths.InitialDirectoryPath);
            var directory = await _storageRepository.GetDirectory(DirectoryPaths.InitialDirectoryPath);
            
            var userInfo = new UserInfoResponseModel
            {
                Login = currentUser.Login,
                UsedStorage = usedStorage,
                CreationDate = directory.CreationDate
            };

            return userInfo;
        }
    }
}
