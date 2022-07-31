using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Institution : IIdentityEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Campo Nome é obrigatório")]
        [MaxLength(255, ErrorMessage = "O Campo Nome deve ter no máximo 255 caracteres")]
        public string? Name { get; set; }

        [MaxLength(255, ErrorMessage = "O Campo Caminho da Imagem deve ter no máximo 255 caracteres")]
        public string? ImagePath { get; set; }
    }
}
