using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.IoC;
using Infrastructure.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ResearchDataset
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddEntityFramework();
            serviceCollection.AddRepositories();
            serviceCollection.AddServices();
            serviceCollection.AddLogging();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>()!;

            // Mock logged user in HttpContext
            var userId = 1;
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, "Mocked User"),
                new Claim(ClaimTypes.Email, "test@test.com.br"),
                new Claim("imagePath", ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var identity = new ClaimsIdentity(authClaims);
            httpContextAccessor.HttpContext = new DefaultHttpContext();
            httpContextAccessor.HttpContext.User = new ClaimsPrincipal(identity);

            var stopWatch = Stopwatch.StartNew();
            var mockDatabase = new MockDatabase(
                provider: serviceProvider,
                datasetPath: "/media/lucas/data/projects/research_files/0704",
                metadataFile: "/media/lucas/data/projects/research_files/metadata/arxiv-metadata-oai-snapshot.json",
                userId: userId,
                institutionId: 1,
                limit: 5
            );

            var task = mockDatabase.Start();
            task.Wait();

            stopWatch.Stop();

            Console.WriteLine("--------- Result Summary --------");
            Console.WriteLine($"Total: {task.Result.Total}");
            Console.WriteLine($"Success: {task.Result.Success}");
            Console.WriteLine($"Errors: {task.Result.Errors}");
            Console.WriteLine($"Time elapsed: {stopWatch.ElapsedMilliseconds / 1000}s");
            Console.WriteLine($"Avg time per file: {(stopWatch.ElapsedMilliseconds / 1000) / task.Result.Total}s");

            if (task.Result.ErrorMessages.Any())
            {
                Console.WriteLine("--------- Error messages --------");
                foreach (var item in task.Result.ErrorMessages)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(item, Formatting.None));
                }
            }
        }
    }
}