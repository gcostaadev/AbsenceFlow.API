namespace AbsenceFlow.API.Models
{
    public class ColaboradorViewModel
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public string EmailCorporativo { get; set; }
        public DateTime DataContratacao { get; set; }
        public int SaldoDiasFerias { get; set; }
        public DateTime CreatedAt { get; set; }

        
        public ColaboradorViewModel(int id, string nomeCompleto, string emailCorporativo, DateTime dataContratacao, int saldoDiasFerias, DateTime createdAt)
        {
            Id = id;
            NomeCompleto = nomeCompleto;
            EmailCorporativo = emailCorporativo;
            DataContratacao = dataContratacao;
            SaldoDiasFerias = saldoDiasFerias;
            CreatedAt = createdAt;
        }
    }
}
