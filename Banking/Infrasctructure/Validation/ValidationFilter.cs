using FluentValidation;

namespace Banking.Api.Infrastructure.Validation
{
    // Filtro genérico que busca el IValidator<T> desde DI y valida el body.
    public sealed class ValidationFilter<T> : IEndpointFilter where T : class
    {
        public async ValueTask<object> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            // Busca el argumento del endpoint que coincida con T (el DTO del request)
            var toValidate = context.Arguments.FirstOrDefault(a => a is T) as T;
            if (toValidate is null)
                return await next(context); // nada que validar (e.g. GET)

            // Resuelve el validador desde DI
            var validator = context.HttpContext.RequestServices.GetService(typeof(IValidator<T>)) as IValidator<T>;
            if (validator is null)
                return await next(context); // si no hay validador, continúa

            var result = await validator.ValidateAsync(toValidate, context.HttpContext.RequestAborted);
            if (result.IsValid)
                return await next(context);

            // Estructura estándar para ValidationProblem
            var errors = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            return Results.ValidationProblem(errors, statusCode: StatusCodes.Status400BadRequest);
        }
    }
}
