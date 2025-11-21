namespace AbsenceFlow.API.Exceptions
{

    public class RecursoNaoEncontradoException : Exception
    {
        
        public RecursoNaoEncontradoException(string message) : base(message)
        {
        }

        
        public RecursoNaoEncontradoException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        
        public RecursoNaoEncontradoException() : base("O recurso solicitado não foi encontrado.")
        {
        }
    }
}
