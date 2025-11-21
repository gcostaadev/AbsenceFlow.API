using AbsenceFlow.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace AbsenceFlow.API.Models
{
    public class StatusUpdateModel
    {
        [Required(ErrorMessage = "O novo status da solicitação é obrigatório.")]
        
        public SolicitacaoStatusEnum NovoStatus { get; set; }
               
    }
}
