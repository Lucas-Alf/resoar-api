using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class KeyWordNewModel
    {
        [Required(ErrorMessage = "O Campo Descrição é obrigatório")]
        [MaxLength(255, ErrorMessage = "O Campo Descrição deve ter no máximo 255 caracteres")]
        public string? Description { get; set; }
    }

    public class KeyWordUpdateModel : KeyWordNewModel
    {
        [Required(ErrorMessage = "O Id é obrigatório")]
        public int? Id { get; set; }
    }
}
