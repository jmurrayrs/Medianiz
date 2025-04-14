# Medianiz

[![NuGet Version](https://img.shields.io/nuget/v/Medianiz.svg)](https://www.nuget.org/packages/Medianiz/)
[![.NET Standard](https://img.shields.io/badge/.NET-Standard%202.1+-blue)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
[![License](https://img.shields.io/badge/license-GPL-green.svg)](LICENSE)

A lightweight implementation of the mediator pattern for .NET applications, designed to simplify in-process messaging.

## Key Features

- Simple request/response handling
- Event publishing to multiple handlers
- First-class async/await support
- CancellationToken integration
- Minimal dependencies
- Works with any DI container

## Installation

```bash
dotnet add package Medianiz
```

## Basic Usage

### 1. Configure Services

```csharp
// In Startup.cs or Program.cs
builder.Services.AddMedianiz(typeof(MyCommandHandler));
```

For multi-project solutions:
```csharp
// Reference assemblies by project name
builder.Services.AddMedianizFromAssemblies("MyApp.Application");
```

### 2. Define Commands and Handlers

**Command:**
```csharp
public record CreateUserCommand(string Email) : IRequest<Guid>;
```

**Handler:**
```csharp
public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
{
    public Task<Guid> Handle(CreateUserCommand request, CancellationToken ct)
    {
        // Business logic here
        return Task.FromResult(Guid.NewGuid());
    }
}
```

### 3. Define Events and Handlers

**Event:**
```csharp
public record UserCreatedEvent(Guid UserId) : INotification;
```

**Handler:**
```csharp
public class UserCreatedHandler : INotificationHandler<UserCreatedEvent>
{
    public Task Handle(UserCreatedEvent notification, CancellationToken ct)
    {
        // React to the event
        return Task.CompletedTask;
    }
}
```

### 4. Use in ASP.NET Core

**Controller Example:**
```csharp
[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IMedianiz _mediator;

    public UsersController(IMedianiz mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetUser), new { id = userId });
    }
}
```

## Advanced Scenarios

### Domain Events

```csharp
public class Order : Entity
{
    public void Complete()
    {
        AddDomainEvent(new OrderCompletedEvent(Id));
    }
}

// Publish events after saving
var order = await _repository.GetOrderAsync();
order.Complete();
await _mediator.PublishDomainEvents(order);
```

### Working with Multiple Assemblies

```csharp
// Create a marker interface in each project
public interface IApplicationMarker { }

// Then register all needed assemblies
builder.Services.AddMedianiz(
    typeof(Application.Marker),
    typeof(Domain.Marker),
    typeof(Infrastructure.Marker));
```

## Configuration Options

### Register Specific Handlers Only

```csharp
services.AddMedianiz(config =>
{
    config.RegisterHandlersFromAssembly(typeof(Program));
    config.RegisterHandler<SpecialHandler>(); // Manual registration
});
```

### Lifecycle Management

```csharp
// Change handler lifetime (default is Transient)
services.AddMedianiz(typeof(Startup))
    .WithHandlerLifetime(ServiceLifetime.Scoped);
```

## Performance Tips

1. **Assembly Filtering**: Only scan necessary assemblies
2. **Marker Interfaces**: Use them to limit handler discovery
3. **Lifetime Management**: Use Scoped for DB-heavy handlers

## Contributing

Pull requests are welcome. For major changes, please open an issue first.

## License

MIT - See [LICENSE](LICENSE) for details.
