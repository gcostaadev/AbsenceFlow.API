using AbsenceFlow.API.Enums;

namespace AbsenceFlow.API.Entities
{
    
    public class Solicitacao : BaseEntity
    {
        
        public Solicitacao(
            int idColaborador,
            SolicitacaoTipoEnum tipo, 
            DateTime dataInicio,
            DateTime dataFim,
            int diasUteisSolicitados,
            string motivo)
        {
            IdColaborador = idColaborador;
            Tipo = tipo;
            DataInicio = dataInicio;
            DataFim = dataFim;
            DiasUteisSolicitados = diasUteisSolicitados;
            Motivo = motivo;
            Status = SolicitacaoStatusEnum.Pendente; 
        }

        
        protected Solicitacao() { }

        
        public int IdColaborador { get; private set; }

        
        public Colaborador Colaborador { get; private set; }


        

        
        public SolicitacaoTipoEnum Tipo { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }
        public int DiasUteisSolicitados { get; private set; }
        public string Motivo { get; private set; }
        public SolicitacaoStatusEnum Status { get; private set; } 


        // 3. Métodos de Negócio (Alteração de Status)

        public void Aprovar()
        {
            if (Status != SolicitacaoStatusEnum.Pendente)
            {
                throw new InvalidOperationException("A solicitação deve estar PENDENTE para ser aprovada.");
            }
            Status = SolicitacaoStatusEnum.Aprovada;
            SetAsUpdated();
        }

        public void Rejeitar()
        {
            if (Status == SolicitacaoStatusEnum.Aprovada)
            {
                
                throw new InvalidOperationException("Solicitações APROVADAS não podem ser diretamente REJEITADAS. O saldo deve ser estornado primeiro.");
            }
            Status = SolicitacaoStatusEnum.Rejeitada;
            SetAsUpdated();
        }
    }
}
