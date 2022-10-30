using Application.Exceptions;
using Application.Interfaces.Services.Domain;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using Domain.Utils;
using Infrastructure.Interfaces.Repositories.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ResearchDataset.Models;

namespace ResearchDataset
{
    public class MockDatabase
    {
        private IServiceProvider _provider;
        private IResearchService _researchService;
        private IKnowledgeAreaService _knowledgeAreaService;
        private IKeyWordService _keyWordService;
        private IUserRepository _userRepository;

        private string _datasetPath { get; set; }
        private string _metadataFile { get; set; }
        private int _limit { get; set; }
        private int _userId { get; set; }

        public MockDatabase(
            IServiceProvider provider,
            string datasetPath,
            string metadataFile,
            int userId,
            int limit
        )
        {
            _provider = provider;
            _researchService = provider.GetService<IResearchService>()!;
            _knowledgeAreaService = provider.GetService<IKnowledgeAreaService>()!;
            _keyWordService = provider.GetService<IKeyWordService>()!;
            _userRepository = provider.GetService<IUserRepository>()!;

            _datasetPath = datasetPath;
            _metadataFile = metadataFile;
            _userId = userId;
            _limit = limit;
        }

        public async Task<MockResult> Start()
        {
            var result = new MockResult();

            using (var fs = new FileStream(_metadataFile, FileMode.Open, FileAccess.Read))
            using (var streamReader = new StreamReader(fs))
            using (var reader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                reader.SupportMultipleContent = true;
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        var metadata = serializer.Deserialize<ArXivMetadata>(reader);
                        if (metadata != null && metadata.Versions != null)
                        {
                            foreach (var revision in metadata.Versions)
                            {
                                var researchPath = $"{_datasetPath}/{metadata.Id}{revision.Version}.pdf";
                                metadata.FilePath = researchPath;
                                var fileInfo = new FileInfo(researchPath);
                                if (!fileInfo.Exists)
                                {
                                    // Console.WriteLine($"File not found {metadata.Id}{revision.Version}.pdf");
                                    continue;
                                }

                                var message = await HandleResearch(metadata);
                                if (!message.Success)
                                    result.ErrorMessages.Add(message);
                                else
                                    result.Success++;
                            }
                        }
                    }

                    if (result.Total == _limit)
                        break;
                }
            }

            return result;
        }

        private async Task<ResponseMessageModel> HandleResearch(ArXivMetadata metadata)
        {
            var domain = new ResearchCreateModel
            {
                Title = metadata.Title?.Replace("\n", "").Trim(),
                Abstract = metadata.Abstract?.Replace("\n", "").Trim(),
                Year = DateTime.Now.Year,
                Type = ResearchType.Article,
                Visibility = ResearchVisibility.Public,
                Language = ResearchLanguage.English,
                InstitutionId = 1,
                KnowledgeAreaIds = new List<int>(),
                KeyWordIds = new List<int>(),
                AuthorIds = new List<int>() { _userId },
                AdvisorIds = new List<int>()
            };

            if (!String.IsNullOrEmpty(metadata.UpdateDate))
                if (DateTime.TryParse(metadata.UpdateDate, out var updateDate))
                    domain.Year = updateDate.Year;

            if (!String.IsNullOrEmpty(metadata.Categories))
            {
                var knowledgeAreaId = HandleKnowledgeArea(metadata.Categories.Trim());
                domain.KnowledgeAreaIds.Add(knowledgeAreaId);

                var keyWordId = HandleKeyWord(metadata.Categories.Trim());
                domain.KeyWordIds.Add(keyWordId);
            }

            if (metadata.AuthorsParsed != null)
            {
                var authorNames = metadata.AuthorsParsed
                    .Select(x => String.Join(" ", x).Trim())
                    .ToList();

                var lastName = authorNames.Last();
                foreach (var name in authorNames)
                {
                    var userId = HandleUser(name);
                    domain.AuthorIds.Add(userId);

                    if (name == lastName)
                        domain.AdvisorIds.Add(userId);
                }
            }

            using (var stream = File.OpenRead(metadata.FilePath!))
            {
                domain.File = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "application/pdf"
                };

                return await _researchService.Add(domain, _userId);
            }
        }

        private int HandleKnowledgeArea(string description)
        {
            var exists = _knowledgeAreaService
                .Query(new FilterBy<KnowledgeArea>(x => x.Description == description))
                .Select(x => (int?)x.Id)
                .FirstOrDefault();

            if (exists.HasValue)
                return exists.Value;

            var domain = new KnowledgeArea
            {
                Description = description,
                CreatedById = _userId
            };

            var result = _knowledgeAreaService.Add(domain);
            if (!result.Success)
                throw new BusinessException($"Error on add Knowledge Area: {result.Message}");

            return domain.Id;
        }

        private int HandleKeyWord(string description)
        {
            var exists = _keyWordService
                .Query(new FilterBy<KeyWord>(x => x.Description == description))
                .Select(x => (int?)x.Id)
                .FirstOrDefault();

            if (exists.HasValue)
                return exists.Value;

            var domain = new KeyWord
            {
                Description = description,
                CreatedById = _userId
            };

            var result = _keyWordService.Add(domain);
            if (!result.Success)
                throw new BusinessException($"Error on add Key Word: {result.Message}");

            return domain.Id;
        }

        private int HandleUser(string name)
        {
            try
            {
                var formattedName = name
                    .Replace(" ", "")
                    .Replace(".", "")
                    .Replace(",", "")
                    .Replace("-", "")
                    .ToLower();

                var email = $"{formattedName}@test.com";

                var exists = _userRepository
                    .Query(new FilterBy<User>(x => x.Email == email))
                    .Select(x => (int?)x.Id)
                    .FirstOrDefault();

                if (exists.HasValue)
                    return exists.Value;

                var domain = new User
                {
                    Name = name,
                    Email = email,
                    Password = "$2a$11$l102IEO02Osx39lnBptYYOdXiJRa9ax2uj7dWSakBPVtilMYo8142",
                    FailLoginCount = 0
                };

                var result = _userRepository.Add(domain);
                return result.Id;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error on add User: {ex.Message}");
            }
        }
    }
}