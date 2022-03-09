
namespace Ordering.API.Application.Behaviors;

public class ValidatorBehavior<TRequest, TRepsonse> : IPipelineBehavior<TRequest, TRepsonse>
{
    private readonly ILogger<ValidatorBehavior<TRequest, TRepsonse>> _logger;
    private readonly IValidator<TRequest>[] _validators;

    public ValidatorBehavior(ILogger<ValidatorBehavior<TRequest, TRepsonse>> logger, IValidator<TRequest>[] validators)
    {
        _logger = logger;
        _validators = validators;
    }

    public async Task<TRepsonse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TRepsonse> next)
    {
        var typeName = request.GetGenericTypeName();

        _logger.LogInformation("----Validating command {CommandType}", typeName);

        var failures = _validators.Select(x => x.Validate(request))
            .SelectMany(x => x.Errors)
            .Where(err => err != null)
            .ToList();

        if (failures.Any())
        {
            _logger.LogWarning("Validations errors - {CommandType} - Command {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);

            throw new OrderingDomainException(
                            $"Command Validation Errors for type {typeof(TRequest).Name}", new ValidationException("Validation exception", failures));
        }

        return await next();
    }
}

