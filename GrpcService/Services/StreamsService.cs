using Grpc.Core;
using System.Text;

namespace GrpcService.Services
{
    public class StreamsService : GrpcService.Services.GrpcStream.GrpcStreamBase
    {
        public override async Task FromServer(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            for (int i = 0; i < int.MaxValue && !context.CancellationToken.IsCancellationRequested; i++)
            {
                await responseStream.WriteAsync(new Response { Message = $"Message {i} from server: {request.Message}" });
                await Task.Delay(1000, context.CancellationToken);
            }

        }


        public override async Task<Response> FromClient(IAsyncStreamReader<Request> requestStream, ServerCallContext context)
        {
            
            var response = new Response();
            StringBuilder stringBuilder = new StringBuilder();

            await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
            {
                stringBuilder.AppendLine(request.Message);
            }
            
            response.Message = stringBuilder.ToString();
            return response;
        }


        public override async Task Bidirectional(IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
            {
                var response = new Response { Message = $"Echo from server: {request.Message}" };
                await responseStream.WriteAsync(response);
            }
        }
    }
}
