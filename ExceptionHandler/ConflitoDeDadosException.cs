namespace AbsenceFlow.API.Exceptions
{

    public class ConflitoDeDadosException : Exception
    {
        
        public ConflitoDeDadosException(string message) : base(message)
        {
        }

        
        public ConflitoDeDadosException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        
        public ConflitoDeDadosException() : base("A operação resultou em um conflito de dados (violando uma restrição de unicidade).")
        {
        }
    }
}
