namespace Mediator.Interfaces
{
    /// <summary>
    /// Marker interface for requests with response
    /// </summary>
    /// <typeparam name="TResponse">Response type</typeparam>
    public interface IRequest<out TResponse> { }

}