using Grpc.Net.Client;
using Grpc.Net.ClientFactory;
using MCIO.Demos.Store.BuildingBlock.Grpc.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MCIO.Demos.Store.BuildingBlock.Grpc.DependencyInjection;
public static class ExtensionMethods
{
    public static IServiceCollection RegisterGrpcClient<TClient>(
        this IServiceCollection services,
        GrpcServiceConfig grpcServiceConfig,
        Action<GrpcClientFactoryOptions>? additionalConfigureGrpcClientFactoryHandler = null,
        Action<GrpcChannelOptions>? additionalConfigureGrpcChannelHandler = null
    ) where TClient : class
    {
        services
            .AddGrpcClient<TClient>(options =>
            {
                options.Address = new Uri(grpcServiceConfig.BaseUrl);
                additionalConfigureGrpcClientFactoryHandler?.Invoke(options);
            })
            .ConfigureChannel(options =>
            {
                options.HttpHandler = new SocketsHttpHandler
                {
                    PooledConnectionIdleTimeout = grpcServiceConfig.PooledConnectionIdleTimeoutInSeconds > 0 
                        ? TimeSpan.FromSeconds(grpcServiceConfig.PooledConnectionIdleTimeoutInSeconds)
                        : Timeout.InfiniteTimeSpan,
                    KeepAlivePingDelay = grpcServiceConfig.KeepAlivePingDelayInSeconds > 0
                        ? TimeSpan.FromSeconds(grpcServiceConfig.KeepAlivePingDelayInSeconds)
                        : Timeout.InfiniteTimeSpan,
                    KeepAlivePingTimeout = grpcServiceConfig.KeepAlivePingTimeoutInSeconds > 0
                        ? TimeSpan.FromSeconds(grpcServiceConfig.KeepAlivePingTimeoutInSeconds)
                        : Timeout.InfiniteTimeSpan,
                    EnableMultipleHttp2Connections = grpcServiceConfig.EnableMultipleHttp2Connections
                };

                additionalConfigureGrpcChannelHandler?.Invoke(options);
            })
            .EnableCallContextPropagation();

        return services;
    }

    public static IServiceCollection RegisterNamedGrpcClient<TClient>(
        this IServiceCollection services,
        GrpcServiceConfig grpcServiceConfig,
        string? name = null,
        Action<GrpcClientFactoryOptions>? additionalConfigureGrpcClientFactoryHandler = null,
        Action<GrpcChannelOptions>? additionalConfigureGrpcChannelHandler = null
    ) where TClient : class
    {

        services
            .AddGrpcClient<TClient>(
                name: name ?? typeof(TClient).FullName!, 
                options =>
                {
                    options.Address = new Uri(grpcServiceConfig.BaseUrl);
                    additionalConfigureGrpcClientFactoryHandler?.Invoke(options);
                }
            )
            .ConfigureChannel(options =>
            {
                options.HttpHandler = new SocketsHttpHandler
                {
                    PooledConnectionIdleTimeout = grpcServiceConfig.PooledConnectionIdleTimeoutInSeconds > 0
                        ? TimeSpan.FromSeconds(grpcServiceConfig.PooledConnectionIdleTimeoutInSeconds)
                        : Timeout.InfiniteTimeSpan,
                    KeepAlivePingDelay = grpcServiceConfig.KeepAlivePingDelayInSeconds > 0
                        ? TimeSpan.FromSeconds(grpcServiceConfig.KeepAlivePingDelayInSeconds)
                        : Timeout.InfiniteTimeSpan,
                    KeepAlivePingTimeout = grpcServiceConfig.KeepAlivePingTimeoutInSeconds > 0
                        ? TimeSpan.FromSeconds(grpcServiceConfig.KeepAlivePingTimeoutInSeconds)
                        : Timeout.InfiniteTimeSpan,
                    EnableMultipleHttp2Connections = grpcServiceConfig.EnableMultipleHttp2Connections
                };

                additionalConfigureGrpcChannelHandler?.Invoke(options);
            })
            .EnableCallContextPropagation();

        return services;
    }
}
