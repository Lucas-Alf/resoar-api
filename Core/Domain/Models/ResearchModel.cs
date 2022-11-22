using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Attributes;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    public class ResearchCreateModel
    {
        [Required(ErrorMessage = "O Campo Titulo é obrigatório")]
        public string? Title { get; set; }

        [MaxLength(2500, ErrorMessage = "O Campo Resumo deve ter no máximo 1000 caracteres")]
        public string? Abstract { get; set; }

        [Required(ErrorMessage = "O Campo Ano é obrigatório")]
        public int? Year { get; set; }

        [Required(ErrorMessage = "O Campo Tipo é obrigatório")]
        public ResearchType? Type { get; set; }

        [Required(ErrorMessage = "O Campo Visibilidade é obrigatório")]
        public ResearchVisibility? Visibility { get; set; }

        [Required(ErrorMessage = "O Campo Idioma é obrigatório")]
        public ResearchLanguage? Language { get; set; }

        [Required(ErrorMessage = "O Campo Instituição é obrigatório")]
        public int? InstitutionId { get; set; }

        [Required(ErrorMessage = "O Campo Autores é obrigatório")]
        public IList<int>? AuthorIds { get; set; }
        public IList<int>? AdvisorIds { get; set; }
        public IList<int>? KeyWordIds { get; set; }
        public IList<int>? KnowledgeAreaIds { get; set; }

        [Required(ErrorMessage = "O Campo Arquivo é obrigatório")]
        [MaxFileSize(20 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".pdf" })]
        public IFormFile? File { get; set; }
    }

    public class ResearchViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Year { get; set; }
        public ResearchType? Type { get; set; }
        public ResearchVisibility? Visibility { get; set; }
        public ResearchLanguage? Language { get; set; }
        public string? LanguageName { get; set; }
        public Guid? FileKey { get; set; }
        public Guid? ThumbnailKey { get; set; }
        public int? InstitutionId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Abstract { get; set; }
        public ResearchCreatedByViewModel? CreatedBy { get; set; }
        public ResearchCreatedByViewModel? Institution { get; set; }

        public IList<AuthorViewModel> Authors { get; set; } = new List<AuthorViewModel>();

        public IList<AdvisorViewModel> Advisors { get; set; } = new List<AdvisorViewModel>();

        public IList<KeyWordViewModel> Keywords { get; set; } = new List<KeyWordViewModel>();

        public IList<KnowledgeAreaViewModel> KnowledgeAreas { get; set; } = new List<KnowledgeAreaViewModel>();
    }

    public class ResearchCreatedByViewModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? ImagePath { get; set; }
    }

    public class ResearchInstitutionViewModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? ImagePath { get; set; }
    }

    [Keyless]
    // [NotMapped]
    public class ResearchFullTextViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Year { get; set; }
        public ResearchType? Type { get; set; }
        public string? LanguageName { get; set; }
        public Guid? FileKey { get; set; }
        public Guid? ThumbnailKey { get; set; }
        public int? InstitutionId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Abstract { get; set; }
        public float? Rank { get; set; }

        [NotMapped]
        public ResearchLanguage? Language { get; set; }

        [NotMapped]
        public IList<AuthorViewModel> Authors { get; set; } = new List<AuthorViewModel>();

        [NotMapped]
        public IList<AdvisorViewModel> Advisors { get; set; } = new List<AdvisorViewModel>();

        [NotMapped]
        public IList<KeyWordViewModel> Keywords { get; set; } = new List<KeyWordViewModel>();

        [NotMapped]
        public IList<KnowledgeAreaViewModel> KnowledgeAreas { get; set; } = new List<KnowledgeAreaViewModel>();
    }

    public class ResearchFullTextQueryModel
    {
        public string? Query { get; set; }
        public int? StartYear { get; set; }
        public int? FinalYear { get; set; }
        public IList<ResearchType>? Types { get; set; }
        public IList<int>? Languages { get; set; }
        public IList<int>? InstitutionIds { get; set; }
        public IList<int>? AuthorIds { get; set; }
        public IList<int>? AdvisorIds { get; set; }
        public IList<int>? KeywordIds { get; set; }
        public IList<int>? KnowledgeAreaIds { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
