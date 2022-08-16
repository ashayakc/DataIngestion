using MusicDataIngestion.Processor;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System.Diagnostics;

namespace MusicDataIngestion
{
    public class Program
    {
        public async static Task<int> Main(String[] args)
        {
            Console.WriteLine("Welcome to music collection data ingestion tool!");
            Console.WriteLine("Initializing components..");

            var watch = new Stopwatch();
            watch.Start();

            var serviceProvider = BuildServiceCollection();
            var processor = new DataProcessor(serviceProvider);
            await processor.ProcessAsync().ConfigureAwait(false);

            watch.Stop();
            var timeSpan = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);
            Console.WriteLine($"Job successfully completed in {timeSpan}");
            return (int)ExitCode.Success;
        }

        private static ServiceProvider BuildServiceCollection()
        {
            var serviceCollection = new ServiceCollection();

            var settings = new ConnectionSettings(new Uri(Settings.ElasticUrl))
                .BasicAuthentication(Settings.ElasticUserName, Settings.ElasticPassword)
                            .DefaultIndex(Settings.IndexName);
            var client = new ElasticClient(settings);
            serviceCollection.AddSingleton<IElasticClient>(provider => client);

            serviceCollection.AddScoped<IDataProcessor, ArtistProcessor>();
            serviceCollection.AddScoped<IDataProcessor, ArtistCollectionProcessor>();
            serviceCollection.AddScoped<IDataProcessor, CollectionMatchProcessor>();
            serviceCollection.AddScoped<IDataProcessor, CollectionProcessor>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}