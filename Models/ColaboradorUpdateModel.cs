using System.ComponentModel.DataAnnotations;

namespace AbsenceFlow.API.Models
{
    public class ColaboradorUpdateModel
    {
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome não pode exceder 150 caracteres.")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O email corporativo é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string EmailCorporativo { get; set; }

    }
}
