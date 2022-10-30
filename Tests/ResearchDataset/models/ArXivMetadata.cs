using Newtonsoft.Json;

namespace ResearchDataset.Models
{
    public class ArXivMetadata
    {
        public string? Id { get; set; }
        public string? Submitter { get; set; }
        public string? Authors { get; set; }
        public string? Title { get; set; }
        public string? Comments { get; set; }

        [JsonProperty(PropertyName = "journal-ref")]
        public string? JournalRef { get; set; }
        public string? Doi { get; set; }

        [JsonProperty(PropertyName = "report-no")]
        public string? ReportNo { get; set; }
        public string? Categories { get; set; }
        public string? License { get; set; }
        public string? Abstract { get; set; }

        [JsonProperty(PropertyName = "update_date")]
        public string? UpdateDate { get; set; }

        [JsonProperty(PropertyName = "authors_parsed")]
        public IList<IList<string>>? AuthorsParsed { get; set; }
        public IList<ArXivMetadataVersion>? Versions { get; set; }
        public string? FilePath { get; set; }
    }

    public class ArXivMetadataVersion
    {
        public string Version { get; set; }
        public string Created { get; set; }
    }
}