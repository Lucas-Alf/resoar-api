using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace Domain.Entities
{
    public class Research : IIdentityEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Campo Titulo é obrigatório")]
        [MaxLength(350, ErrorMessage = "O Campo Titulo deve ter no máximo 350 caracteres")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "O Campo Ano é obrigatório")]
        public int? Year { get; set; }

        [Required(ErrorMessage = "O Campo Tipo é obrigatório")]
        public ResearchTypeEnum? Type { get; set; }

        [Required(ErrorMessage = "O Campo Visibilidade é obrigatório")]
        public ResearchVisibilityEnum? Visibility { get; set; }

        [Required(ErrorMessage = "O Campo Idioma é obrigatório")]
        [MaxLength(80, ErrorMessage = "O Campo Idioma deve ter no máximo 80 caracteres")]
        public string? Language { get; set; }

        [Required(ErrorMessage = "O Campo Caminho do Arquivo é obrigatório")]
        [MaxLength(350, ErrorMessage = "O Campo Caminho do Arquivo deve ter no máximo 350 caracteres")]
        public string? FilePath { get; set; }

        [Required(ErrorMessage = "O Campo Caminho do Thumbnail é obrigatório")]
        [MaxLength(350, ErrorMessage = "O Campo Caminho do Thumbnail deve ter no máximo 350 caracteres")]
        public string? ThumbnailPath { get; set; }

        public NpgsqlTsVector? SearchVector { get; set; }

        public string? RawContent { get; set; }

        [Required(ErrorMessage = "O Campo Instituição é obrigatório")]
        public int? InstitutionId { get; set; }

        [Required(ErrorMessage = "O Campo Data de Criação é obrigatório")]
        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        #region references
        public virtual Institution? Institution { get; set; }

        #endregion references

        #region collections
        public virtual ICollection<ResearchAuthor>? Authors { get; set; }
        public virtual ICollection<ResearchAdvisor>? Advisors { get; set; }
        public virtual ICollection<ResearchAdvisorApproval>? ResearchAdvisorApprovals { get; set; }
        public virtual ICollection<ResearchKnowledgeArea>? ResearchKnowledgeAreas { get; set; }
        public virtual ICollection<ResearchKeyWord>? ResearchKeyWords { get; set; }

        #endregion collections
    }
}
