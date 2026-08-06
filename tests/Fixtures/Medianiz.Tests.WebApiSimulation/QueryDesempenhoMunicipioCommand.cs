using Mediator.Interfaces;

namespace Medianiz.Tests.WebApiSimulation;

public sealed record QueryDesempenhoMunicipioCommand(int MunicipioId) : IRequest<string>;
