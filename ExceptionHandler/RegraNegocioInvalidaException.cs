namespace AbsenceFlow.API.Exceptions
{

    public class RegraNegocioInvalidaException : Exception
    {
        
        public RegraNegocioInvalidaException(string message) : base(message)
        {
        }

        
        public RegraNegocioInvalidaException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        
        public RegraNegocioInvalidaException() : base("A operação viola uma regra de negócio do sistema.")
        {
        }
    }
}
