using FileStorage.BLL.Queries;
using FileStorage.DAL;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.QueryHandlers
{
    class IsAuthenticatedQueryHandler : IRequestHandler<IsAuthenticatedQuery, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly CurrentUser _currentUser;

        public IsAuthenticatedQueryHandler(IUserRepository userRepository, CurrentUser currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(IsAuthenticatedQuery request, CancellationToken cancellationToken)
        {
            int userId = await _userRepository.Authenticate(request.Login, request.Password);

            _currentUser.InitialiseUser(userId, request.Login);

            return true;
        }
    }
}
