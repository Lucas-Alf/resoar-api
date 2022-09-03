using Domain.Enums;

namespace Domain.Models
{
    public class ResearchModel
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public ResearchTypeEnum? Type { get; set; }
        public ResearchVisibilityEnum? Visibility { get; set; }
        public string? Language { get; set; }
        public string? FilePath { get; set; }
        public string? ThumbnailPath { get; set; }
        public IList<AuthorModel>? Authors { get; set; }
        public InstitutionModel? Institution { get; set; }
    }
}
