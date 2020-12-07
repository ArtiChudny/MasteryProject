using FileStorage.BLL.Queries;
using FileStorage.DAL.Models;
using FileStorage.DAL.Repositories.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.QueryHandlers
{
    class IsAuthenticatedQueryHandler : IRequestHandler<IsAuthenticatedQuery, bool>
    {
        private readonly IUserRepository _userRepository;

        public IsAuthenticatedQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(IsAuthenticatedQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUser();

            return (request.Login == user.Login && request.Password == user.Password);
        }
    }
}
