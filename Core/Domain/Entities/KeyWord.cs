using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class KeyWord : IIdentityEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Campo Descrição é obrigatório")]
        [MaxLength(255, ErrorMessage = "O Campo Descrição deve ter no máximo 255 caracteres")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "O Campo usuário criador é obrigatório")]
        public long? CreatedById { get; set; }
        public long? ModifiedById { get; set; }
    }
}
