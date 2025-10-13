using LinoVative.Service.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LinoVative.Service.Backend.MediaTRs
{
    public sealed class Mediator : IMediator
    {
        private readonly IServiceProvider _sp;
        public Mediator(IServiceProvider sp) => _sp = sp;

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var handler = _sp.GetRequiredService(handlerType);

            var behaviors = GetBehaviors(request.GetType(), typeof(TResponse));
            // Build pipeline: behaviors -> handler
            Func<object, CancellationToken, Task<TResponse>> terminal = (req, token) =>
                (Task<TResponse>)handlerType
                    .GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.Handle))!
                    .Invoke(handler, new[] { req, token })!;

            Func<object, CancellationToken, Task<TResponse>> pipeline = terminal;

            // Run behaviors in reverse registration order (outermost first)
            foreach (var behavior in behaviors.Reverse())
            {
                var beh = behavior; // close over
                var handleMethod = beh.GetType().GetMethod("Handle")!;
                var nextCopy = pipeline;

                pipeline = (req, token) => (Task<TResponse>)handleMethod.Invoke(
                    beh,
                    new object[] { req, token, new Func<object, CancellationToken, Task<TResponse>>((r, t) => nextCopy(r, t)) }
                )!;
            }

            return pipeline(request, ct);
        }

        private IEnumerable<object> GetBehaviors(Type requestType, Type responseType)
        {
            var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType);
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(behaviorType);
            return (IEnumerable<object>)_sp.GetService(enumerableType) ?? Array.Empty<object>();
        }
    }


    public sealed class LoggingBehavior<TReq, TRes> : IPipelineBehavior<TReq, TRes>
    where TReq : IRequest<TRes>
    {
        private readonly ILogger<LoggingBehavior<TReq, TRes>> _log;
        public LoggingBehavior(ILogger<LoggingBehavior<TReq, TRes>> log) => _log = log;

        public async Task<TRes> Handle(TReq req, CancellationToken ct, Func<TReq, CancellationToken, Task<TRes>> next)
        {
            _log.LogInformation("Handling {Request}", typeof(TReq).Name);
            var res = await next(req, ct);
            _log.LogInformation("Handled {Request}", typeof(TReq).Name);
            return res;
        }
    }
}
