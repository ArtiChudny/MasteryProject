using FileStorage.BLL.Queries;
using FileStorage.DAL.Constants;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileStorage.BLL.Handlers.QueryHandlers
{
    class GetFileExportFormatsQueryHandler : IRequestHandler<GetFileExportFormatsQuery, string[]>
    {
        public Task<string[]> Handle(GetFileExportFormatsQuery request, CancellationToken cancellationToken)
        {
            string[] formats = { FileFormats.Json, FileFormats.Xml };

            return Task.FromResult(formats);
        }
    }
}
