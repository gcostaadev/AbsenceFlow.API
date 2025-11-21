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

        // 1. Campos de Dados
        public string NomeCompleto { get; private set; }
        public string EmailCorporativo { get; private set; }
        public DateTime DataContratacao { get; private set; }

        // 2. Saldo (Lógica de Negócio)
        public int SaldoDiasFerias { get; private set; }

        // 3. Relacionamento (Coleção de Solicitações)
       
        private readonly List<Solicitacao> _solicitacoes;
        public IReadOnlyCollection<Solicitacao> Solicitacoes => _solicitacoes;


        // 4. Métodos de Negócio (Atualização)

        public void AtualizarSaldoFerias(int dias, bool isDeducao)
        {
            if (isDeducao)
            {
                SaldoDiasFerias -= dias;
            }
            else
            {
                // Estorno (devolve dias ao saldo)
                SaldoDiasFerias += dias;
            }
            // Chama o método da BaseEntity para atualizar a data de modificação
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
