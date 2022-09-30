using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
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
        public Guid? FileKey { get; set; }

        [Required(ErrorMessage = "O Campo Caminho do Thumbnail é obrigatório")]
        public Guid? ThumbnailKey { get; set; }

        public NpgsqlTsVector? SearchVector { get; set; }

        public string? RawContent { get; set; }

        [MaxLength(1000, ErrorMessage = "O Campo Abstract deve ter no máximo 1000 caracteres")]
        public string? Abstract { get; set; }

        [Required(ErrorMessage = "O Campo Instituição é obrigatório")]
        public int? InstitutionId { get; set; }

        [Required(ErrorMessage = "O Campo Usuário Criador é obrigatório")]
        public int? CreatedById { get; set; }

        [Required(ErrorMessage = "O Campo Data de Criação é obrigatório")]
        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        #region references
        public virtual Institution? Institution { get; set; }
        public virtual User? CreatedBy { get; set; }

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
