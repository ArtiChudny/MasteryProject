using FileStorage.BLL.Models.ResponceModels.QueryResponceModels;
using FileStorage.BLL.Queries;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.QueryHandlers
{
    public class GetDirectoryInfoQueryHandler : IRequestHandler<GetDirectoryInfoQuery, DirectoryInfoResponseModel>
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IUserRepository _userRepository;

        public GetDirectoryInfoQueryHandler(IStorageRepository storageRepository, IUserRepository userRepository)
        {
            _storageRepository = storageRepository;
            _userRepository = userRepository;
        }

        public async Task<DirectoryInfoResponseModel> Handle(GetDirectoryInfoQuery request, CancellationToken cancellationToken)
        {
            var directory = await _storageRepository.GetDirectory(request.Path);
            long directorySize = await _storageRepository.GetUsedStorage(directory.Path);

            var currentUser = await _userRepository.GetUser();

            var responseModel = new DirectoryInfoResponseModel()
            {
                CreationDate = directory.CreationDate,
                ModificationDate = directory.ModificationDate,
                Name = directory.Name,
                Path = directory.Path,
                Size = directorySize,
                Login = currentUser.Login
            };

            return responseModel;
        } 
    }
}
