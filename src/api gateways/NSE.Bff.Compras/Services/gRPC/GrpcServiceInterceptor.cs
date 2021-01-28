using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace NSE.Bff.Compras.Services.gRPC
{
    public class GrpcServiceInterceptor : Interceptor
    {
        private readonly ILogger<GrpcServiceInterceptor> _logger;
        private readonly IHttpContextAccessor _httpContextAccesor;

        public GrpcServiceInterceptor(
            ILogger<GrpcServiceInterceptor> logger,
            IHttpContextAccessor httpContextAccesor)
        {
            _logger = logger;
            _httpContextAccesor = httpContextAccesor;
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request, 
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var token = _httpContextAccesor.HttpContext.Request.Headers["Authorization"];

            var headers = new Metadata
            {
                {"Authorization", token}
            };

            var options = context.Options.WithHeaders(headers);
            context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);

            return base.AsyncUnaryCall(request, context, continuation);
        }
    }
}