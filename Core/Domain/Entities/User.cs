using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class User : IIdentityEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Campo Nome é obrigatório")]
        [MaxLength(255, ErrorMessage = "O Campo Nome deve ter no máximo 255 caracteres")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "O Campo Email é obrigatório")]
        [EmailAddress(ErrorMessage = "O Campo Email não é válido")]
        [MaxLength(255)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O Campo Senha é obrigatório")]
        [MaxLength(60, ErrorMessage = "O Campo Senha deve ter no máximo 60 caracteres")]
        public string? Password { get; set; }

        [MaxLength(255, ErrorMessage = "O Campo Caminho da Imagem deve ter no máximo 255 caracteres")]
        public string? ImagePath { get; set; }
        public int FailLoginCount { get; set; }
        public DateTime? LastLogin { get; set; }

        #region collections
        public virtual ICollection<ResearchAuthor>? ResearchAuthors { get; set; }
        public virtual ICollection<UserSavedResearch>? UserSavedResearch { get; set; }

        #endregion collections
    }
}
