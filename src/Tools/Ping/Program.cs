using Grpc.Net.Client;
using MCIO.Demos.Store.Commom.Protos.V1;
using System.Reflection;
using System.Text.Json;

var testUrl = "http://localhost:6032";
using var testChannel = GrpcChannel.ForAddress(testUrl);
var testOrigin = "test";

var testPingRequest = new PingRequest
{
    ExecutionInfo = new ExecutionInfo
    {
        CorrelationId = Guid.NewGuid().ToString(),
        Origin = testOrigin,
        TenantCode = Guid.NewGuid().ToString(),
        User = testOrigin
    }
};
var testClient = new MCIO.Demos.Store.Ports.AdminMobileBFF.Protos.V1.PingService.PingServiceClient(testChannel);

Console.WriteLine("Test ping start");
var reply = await testClient.PingAsync(testPingRequest);

Console.WriteLine(JsonSerializer.Serialize(reply));
Console.WriteLine("Test ping end");

Console.ReadLine();

return;

var httpUrl1 = "http://ports-admin-mobile-bff-grpc.localhost:5000/api/v1/ping";
var httpUrl2 = "http://ports-admin-web-bff-grpc.localhost:5000/api/v1/ping";
var httpUrl3 = "http://ports-client-mobile-bff-grpc.localhost:5000/api/v1/ping";
var httpUrl4 = "http://ports-client-web-bff-grpc.localhost:5000/api/v1/ping";

var httpClient = new HttpClient();

var grpcUrl1 = "http://ports-admin-mobile-bff-grpc.localhost:5001/";
var grpcUrl2 = "http://ports-admin-web-bff-grpc.localhost:5001/";
var grpcUrl3 = "http://ports-client-mobile-bff-grpc.localhost:5001/";
var grpcUrl4 = "http://ports-client-web-bff-grpc.localhost:5001/";

using var channel1 = GrpcChannel.ForAddress(grpcUrl1);
using var channel2 = GrpcChannel.ForAddress(grpcUrl2);
using var channel3 = GrpcChannel.ForAddress(grpcUrl3);
using var channel4 = GrpcChannel.ForAddress(grpcUrl4);

var client1 = new MCIO.Demos.Store.Ports.AdminMobileBFF.Protos.V1.PingService.PingServiceClient(channel1);
var client2 = new MCIO.Demos.Store.Ports.AdminWebBFF.Protos.V1.PingService.PingServiceClient(channel2);
var client3 = new MCIO.Demos.Store.Ports.ClientMobileBFF.Protos.V1.PingService.PingServiceClient(channel3);
var client4 = new MCIO.Demos.Store.Ports.ClientWebBFF.Protos.V1.PingService.PingServiceClient(channel4);

var origin = Assembly.GetExecutingAssembly().GetName().Name;

var pingRequest = new PingRequest
{
    ExecutionInfo = new ExecutionInfo { 
        CorrelationId = Guid.NewGuid().ToString(),
        Origin = origin,
        TenantCode = Guid.NewGuid().ToString(),
        User = origin
    }
};

await Parallel.ForAsync(
    fromInclusive: 0,
    toExclusive: 10,
    cancellationToken: CancellationToken.None,
    body: async (i, cancellationToken) =>
    {
        Console.WriteLine(httpUrl1);
        await httpClient.GetAsync(httpUrl1);

        Console.WriteLine(httpUrl2);
        await httpClient.GetAsync(httpUrl2);

        Console.WriteLine(httpUrl3);
        await httpClient.GetAsync(httpUrl3);

        Console.WriteLine(httpUrl4);
        await httpClient.GetAsync(httpUrl4);

        Console.WriteLine(grpcUrl1);
        await client1.PingAsync(pingRequest);

        Console.WriteLine(grpcUrl2);
        await client2.PingAsync(pingRequest);

        Console.WriteLine(grpcUrl3);
        await client3.PingAsync(pingRequest);

        Console.WriteLine(grpcUrl4);
        await client4.PingAsync(pingRequest);

        Console.WriteLine("-------------------------");
    }
);


Console.WriteLine("\nPress [ENTER] to exit...");
Console.ReadLine();