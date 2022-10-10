using System.ComponentModel.DataAnnotations;
using Domain.Attributes;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Domain.Models
{
    public class ResearchCreateModel
    {
        [Required(ErrorMessage = "O Campo Titulo é obrigatório")]
        public string? Title { get; set; }

        [MaxLength(1000, ErrorMessage = "O Campo Abstract deve ter no máximo 1000 caracteres")]
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

        [Required(ErrorMessage = "O Campo Arquivo é obrigatório")]
        [MaxFileSize(20 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".pdf" })]
        public IFormFile? File { get; set; }
    }
}
