using E_Commerce.Domain.Exceptions;
using Extensions;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Interceptors;

public class HandleErrorInterceptor(ILogger<HandleErrorInterceptor> logger) : Interceptor
{
    private readonly ILogger<HandleErrorInterceptor> _logger = logger;

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception e)
        {
            switch (e)
            {
                case AppException appException:
                    throw new RpcException(new Status(appException.StatusCode.ToRpcStatusCode(), appException.Message));

                default:
                    _logger.LogError(e, $"An error occurred when calling {context.Method}");
                    throw new RpcException(new Status(StatusCode.Unknown, e.Message));
            }
        }
    }
}
