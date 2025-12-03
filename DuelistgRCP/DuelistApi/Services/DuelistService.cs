using Grpc.Core;
using DuelistApi.Mappers;
using DuelistApi.Models;
using DuelistApi.Repositories;

namespace DuelistApi.Services;

public class DuelistService : DuelistApi.DuelistService.DuelistServiceBase
{
    private readonly IDuelistRepository _repository;

    public DuelistService(IDuelistRepository repository)
    {
        _repository = repository;
    }

    public override async Task<CreateDuelistResponse> CreateDuelists(
        IAsyncStreamReader<CreateDuelistRequest> requestStream,
        ServerCallContext context)
    {
        var created = new List<DuelistResponse>();

        while (await requestStream.MoveNext())
        {
            var duelist = requestStream.Current.ToModel();
            var result = await _repository.CreateAsync(duelist, context.CancellationToken);
            created.Add(result.ToResponse());
        }

        return new CreateDuelistResponse
        {
            SuccessCount = created.Count,
            Duelists = { created }
        };
    }

    public override async Task<DuelistResponse> GetDuelistById(DuelistByIdRequest request, ServerCallContext context)
    {
        var duelist = await _repository.GetByIdAsync(request.Id, context.CancellationToken);
        return duelist?.ToResponse() ?? throw new RpcException(new Status(StatusCode.NotFound, "Duelist not found"));
    }

}
