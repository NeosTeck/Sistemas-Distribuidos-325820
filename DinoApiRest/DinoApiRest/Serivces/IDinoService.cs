using System;
using System.Threading;
using System.Threading.Tasks;
using DinoApiRest.Dtos;

namespace DinoApiRest.Services
{
    public interface IDinoService
    {
        Task<DinoDto> GetDinoByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
