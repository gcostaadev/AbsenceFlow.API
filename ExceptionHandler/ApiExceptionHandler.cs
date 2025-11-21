using AbsenceFlow.API.Exceptions; // Garanta que este namespace existe e contém suas exceções
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AbsenceFlow.API.ExceptionHandler
{
    // A classe deve implementar a interface IExceptionHandler
    public class ApiExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ApiExceptionHandler> _logger;

        // Injeção do Logger (Opcional, mas altamente recomendado para logs de erro)
        public ApiExceptionHandler(ILogger<ApiExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // 1. Loga o erro completo no console/arquivo de logs
            _logger.LogError(
                exception,
                "Ocorreu uma exceção: {Message}",
                exception.Message);

            // 2. Mapeia a exceção personalizada para o StatusCode e Título do ProblemDetails
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

            // 4. Define o StatusCode e serializa a resposta
            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);

            
            return true;
        }
    }
}
