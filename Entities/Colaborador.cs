namespace AbsenceFlow.API.Entities
{
    
    public class Colaborador : BaseEntity
    {
        
        public Colaborador(string nomeCompleto, string emailCorporativo, DateTime dataContratacao)
        {
            NomeCompleto = nomeCompleto;
            EmailCorporativo = emailCorporativo;
            DataContratacao = dataContratacao;
            SaldoDiasFerias = 30; 

            
            _solicitacoes = new List<Solicitacao>();
        }

        
        protected Colaborador() { }

        
        public string NomeCompleto { get; private set; }
        public string EmailCorporativo { get; private set; }
        public DateTime DataContratacao { get; private set; }

        
        public int SaldoDiasFerias { get; private set; }

        
       
        private readonly List<Solicitacao> _solicitacoes;
        public IReadOnlyCollection<Solicitacao> Solicitacoes => _solicitacoes;
               

        public void AtualizarSaldoFerias(int dias, bool isDeducao)
        {
            if (isDeducao)
            {
                SaldoDiasFerias -= dias;
            }
            else
            {
                
                SaldoDiasFerias += dias;
            }
            
            SetAsUpdated();
        }

        public void AtualizarInformacoes(string nome, string email)
        {
            NomeCompleto = nome;
            EmailCorporativo = email;
            SetAsUpdated();
        }
    }
}
