using AbsenceFlow.API.Exceptions; 
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AbsenceFlow.API.ExceptionHandler
{
    
    public class ApiExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ApiExceptionHandler> _logger;

        
        public ApiExceptionHandler(ILogger<ApiExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            
            _logger.LogError(
                exception,
                "Ocorreu uma exceção: {Message}",
                exception.Message);

            
            var (statusCode, title) = exception switch
            {
                
                RecursoNaoEncontradoException => (StatusCodes.Status404NotFound, "Recurso Não Encontrado"),
                RegraNegocioInvalidaException => (StatusCodes.Status400BadRequest, "Regra de Negócio Inválida"),
                ConflitoDeDadosException => (StatusCodes.Status409Conflict, "Conflito de Dados"),

                
                _ => (StatusCodes.Status500InternalServerError, "Erro Interno do Servidor")
            };

            
            var details = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message,
                
                Type = exception.GetType().Name
            };

            
            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);

            
            return true;
        }
    }
}
