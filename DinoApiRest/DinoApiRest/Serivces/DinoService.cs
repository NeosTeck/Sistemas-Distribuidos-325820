using System;
using System.Threading;
using System.Threading.Tasks;
using DinoApiRest.Dtos;
using DinoApiRest.Gateways;
using DinoApiRest.Exceptions;

namespace DinoApiRest.Services
{
    public class DinoService : IDinoService
    {
        private readonly IDinoGateway _dinoGateway;

        public DinoService(IDinoGateway dinoGateway)
        {
            _dinoGateway = dinoGateway;
        }

        public async Task<DinoDto> GetDinoByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var dino = await _dinoGateway.GetDinoByIdAsync(id, cancellationToken);

            if (dino == null)
                throw new DinoNotFoundException(id);

            return new DinoDto
            {
                Id = dino.Id,
                Nombre = dino.Nombre,
                Orden = dino.Orden,
                Postura = dino.Postura,
                PeriodoPpl = dino.PeriodoPpl,
                Dieta = dino.Dieta,
                Continente = dino.Continente
            };
        }
    }
}
