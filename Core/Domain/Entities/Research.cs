﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int? Type { get; set; }

        [Required(ErrorMessage = "O Campo Visibilidade é obrigatório")]
        public int? Visibility { get; set; }

        [Required(ErrorMessage = "O Campo Idioma é obrigatório")]
        public string? Language { get; set; }

        [Required(ErrorMessage = "O Campo Caminho do Arquivo é obrigatório")]
        public string? FilePath { get; set; }

        [Required(ErrorMessage = "O Campo Caminho do Thumbnail é obrigatório")]
        public string? ThumbnailPath { get; set; }

        [Required(ErrorMessage = "O Campo Vetor de Pesquisa é obrigatório")]
        public NpgsqlTsVector? FileVector { get; set; }

        [MaxLength(1000, ErrorMessage = "O Campo Resumo deve ter no máximo 1000 caracteres")]
        public string? Abstract { get; set; }

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