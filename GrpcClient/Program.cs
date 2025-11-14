
using Grpc.Net.Client;
using GrpcService.Services;

using var channel = GrpcChannel.ForAddress("https://localhost:7108");
var client = new GrpcService.Services.PeopleGrpcService.PeopleGrpcServiceClient(channel);

var people = await client.ReadAsync(new Google.Protobuf.WellKnownTypes.Empty());


foreach (var person in people.Collection)
{
    Console.WriteLine($"Id: {person.Id}, Name: {person.FirstName} {person.LastName}, Age: {person.Age}");
}

Console.ReadLine();

try
{
    var p = await client.ReadByIdAsync(new Id { Value = 44 });

}
catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
{
    Console.WriteLine(ex.Status.Detail);
}

Console.ReadLine();

var op1 = await client.ReadByIdOptionalAsync(new Id { Value = 2 });

var op2 = await client.ReadByIdOptionalAsync(new Id { Value = 44 });

Console.ReadLine();

