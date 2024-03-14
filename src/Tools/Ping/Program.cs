using Grpc.Net.Client;
using System.Reflection;

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

var client1 = new MCIO.Demos.Store.Ports.AdminMobileBFF.PingService.PingServiceClient(channel1);
var client2 = new MCIO.Demos.Store.Ports.AdminWebBFF.PingService.PingServiceClient(channel2);
var client3 = new MCIO.Demos.Store.Ports.ClientMobileBFF.PingService.PingServiceClient(channel3);
var client4 = new MCIO.Demos.Store.Ports.ClientWebBFF.PingService.PingServiceClient(channel4);

var origin = Assembly.GetExecutingAssembly().GetName().Name;

var request1 = new MCIO.Demos.Store.Ports.AdminMobileBFF.PingRequest() { Origin = origin };
var request2 = new MCIO.Demos.Store.Ports.AdminWebBFF.PingRequest() { Origin = origin };
var request3 = new MCIO.Demos.Store.Ports.ClientMobileBFF.PingRequest() { Origin = origin };
var request4 = new MCIO.Demos.Store.Ports.ClientWebBFF.PingRequest() { Origin = origin };


for (int i = 0; i < 10; i++)
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
    await client1.PingAsync(request1);

    Console.WriteLine(grpcUrl2);
    await client2.PingAsync(request2);

    Console.WriteLine(grpcUrl3);
    await client3.PingAsync(request3);

    Console.WriteLine(grpcUrl4);
    await client4.PingAsync(request4);

    Console.WriteLine("-------------------------");
}

Console.WriteLine("\nPress [ENTER] to exit...");
Console.ReadLine();