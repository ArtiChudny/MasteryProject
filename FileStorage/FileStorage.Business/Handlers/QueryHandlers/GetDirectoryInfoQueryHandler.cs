using FileStorage.BLL.Models.ResponceModels.QueryResponceModels;
using FileStorage.BLL.Queries;
using FileStorage.DAL.Models;
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
            long directorySize = 0;
            CalculateDirectorySize(directory, ref directorySize);

            var currentUser = await _userRepository.GetUser();

            var responseModel = new DirectoryInfoResponseModel()
            {
                CreationDate = directory.CreationDate,
                ModificationDate = directory.ModificationDate,
                Name = directory.Name,
                Path = request.Path,
                Size = directorySize,
                Login = currentUser.Login
            };

            return responseModel;
        }

        private void CalculateDirectorySize(StorageDirectory directory, ref long directorySize)
        {
            foreach (var file in directory.Files)
            {
                directorySize += file.Value.Size;
            }

            foreach (var dir in directory.Directories)
            {
                foreach (var file in dir.Value.Files)
                {
                    directorySize += file.Value.Size;
                }
                CalculateDirectorySize(dir.Value, ref directorySize);
            }
        }
    }
}
