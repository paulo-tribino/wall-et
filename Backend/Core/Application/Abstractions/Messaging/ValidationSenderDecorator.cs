using Application.Errors;
using FluentValidation;
using SharedKernel;

namespace Application.Abstractions.Messaging;

internal sealed class ValidationSenderDecorator : ISender
{
    private readonly ISender _sender;
    private readonly IServiceProvider _serviceProvider;

    public ValidationSenderDecorator(
        ISender sender,
        IServiceProvider serviceProvider)
    {
        _sender = sender;
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<TResponse>> SendAsync<TResponse>(
        IQuery<TResponse> query,
        CancellationToken cancellationToken)
    {
        var error = await ValidateAsync(query, cancellationToken);

        if (error is not null)
        {
            return Result.Failure<TResponse>(error);
        }

        return await _sender.SendAsync(query, cancellationToken);
    }

    public async Task<Result<TResponse>> SendAsync<TResponse>(
        ICommand<TResponse> command,
        CancellationToken cancellationToken)
    {
        var error = await ValidateAsync(command, cancellationToken);

        if (error is not null)
        {
            return Result.Failure<TResponse>(error);
        }

        return await _sender.SendAsync(command, cancellationToken);
    }

    public async Task<Result> SendAsync(
        ICommand command,
        CancellationToken cancellationToken)
    {
        var error = await ValidateAsync(command, cancellationToken);

        if (error is not null)
        {
            return Result.Failure(error);
        }

        return await _sender.SendAsync(command, cancellationToken);
    }

    private async Task<Error?> ValidateAsync(
        object request,
        CancellationToken cancellationToken)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(request.GetType());
        var validator = _serviceProvider.GetService(validatorType) as IValidator;

        if (validator is null)
        {
            return null;
        }

        var validationContext = (IValidationContext)Activator.CreateInstance(
            typeof(ValidationContext<>).MakeGenericType(request.GetType()),
            request)!;

        var validationResult = await validator.ValidateAsync(validationContext, cancellationToken);

        return validationResult.IsValid
            ? null
            : ValidationErrors.BadRequest(
                string.Join(
                    ", ",
                    validationResult.Errors.Select(error => error.ErrorMessage)));
    }
}
