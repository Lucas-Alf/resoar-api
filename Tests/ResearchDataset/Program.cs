using System.Diagnostics;
using Application.IoC;
using Infrastructure.IoC;
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

            Console.WriteLine("Running...");
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var mockDatabase = new MockDatabase(
                provider: serviceProvider,
                datasetPath: "/media/lucas/data/projects/research_files/1902",
                metadataFile: "/media/lucas/data/projects/research_files/metadata/arxiv-metadata-oai-snapshot.json",
                userId: 1,
                limit: 10
            );

            var task = mockDatabase.Start();
            task.Wait();

            stopWatch.Stop();

            Console.WriteLine("--------- Result Summary --------");
            Console.WriteLine($"Total: {task.Result.Total}");
            Console.WriteLine($"Success: {task.Result.Success}");
            Console.WriteLine($"Errors: {task.Result.Errors}");
            Console.WriteLine($"Time elapsed: {stopWatch.ElapsedMilliseconds}ms");

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