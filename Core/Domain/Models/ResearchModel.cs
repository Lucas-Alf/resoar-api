using System.ComponentModel.DataAnnotations;
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

    [Keyless]
    public class ResearchFullTextModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int Year { get; set; }
        public int Type { get; set; }
        public int Visibility { get; set; }
        public string? Language { get; set; }
        public Guid FileKey { get; set; }
        public Guid ThumbnailKey { get; set; }
        public int InstitutionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Abstract { get; set; }
        public float Rank { get; set; }
        public IList<AuthorViewModel> Authors { get; set; } = new List<AuthorViewModel>();
        public IList<AdvisorViewModel> Advisors { get; set; } = new List<AdvisorViewModel>();
        public IList<KeyWordViewModel> Keywords { get; set; } = new List<KeyWordViewModel>();
        public IList<KnowledgeAreaViewModel> KnowledgeAreas { get; set; } = new List<KnowledgeAreaViewModel>();
    }
}
