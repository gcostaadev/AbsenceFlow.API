using AbsenceFlow.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace AbsenceFlow.API.Models
{
    public class SolicitacaoInputModel
    {
        [Required(ErrorMessage = "O ID do colaborador é obrigatório.")]
        public int IdColaborador { get; set; }

        [Required(ErrorMessage = "O tipo de solicitação (Férias ou Ausência) é obrigatório.")]
        public SolicitacaoTipoEnum Tipo { get; set; }

        [Required(ErrorMessage = "A data de início da ausência é obrigatória.")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A data de fim da ausência é obrigatória.")]
        public DateTime DataFim { get; set; }

        [Required(ErrorMessage = "O motivo da solicitação é obrigatório.")]
        [StringLength(255, ErrorMessage = "O motivo não pode exceder 255 caracteres.")]
        public string Motivo { get; set; }
    }
}
