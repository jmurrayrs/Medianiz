using Mediator.Interfaces;

namespace Medianiz.Tests.WebApiSimulation;

public sealed class QueryDesempenhoMunicipioCommandHandler
    : IRequestHandler<QueryDesempenhoMunicipioCommand, string>
{
    public Task<string> Handle(QueryDesempenhoMunicipioCommand request, CancellationToken ct)
        => Task.FromResult($"municipio:{request.MunicipioId}");
}
