using MusicDataIngestion.Constants;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace MusicDataIngestion.Processor
{
    public class DataProcessor
    {
        private readonly IEnumerable<IDataProcessor> _dataProcessors;
        private readonly IElasticClient _elasticClient;
        public DataProcessor(ServiceProvider serviceProvider)
        {
            _dataProcessors = serviceProvider.GetServices<IDataProcessor>();
            _elasticClient = serviceProvider.GetRequiredService<IElasticClient>();
        }

        public async Task ProcessAsync()
        {
            Console.WriteLine("Analysing artist data..");
            var artistProcessor = _dataProcessors.FirstOrDefault(processor => processor.DataType.Equals(Keys.ARTIST, StringComparison.OrdinalIgnoreCase));
            await artistProcessor!.ProcessAsync().ConfigureAwait(false);
            Console.WriteLine("Artist data analysing complete");

            Console.WriteLine("Analysing artist collection data..");
            var artistCollectionProcessor = _dataProcessors.FirstOrDefault(processor => processor.DataType.Equals(Keys.ARTIST_COLLECTION, StringComparison.OrdinalIgnoreCase));
            await artistCollectionProcessor!.ProcessAsync().ConfigureAwait(false);
            Console.WriteLine("Artist collection data analysing complete");

            Console.WriteLine("Analysing collection match data..");
            var collectionMatchProcessor = _dataProcessors.FirstOrDefault(processor => processor.DataType.Equals(Keys.COLLECTION_MATCH, StringComparison.OrdinalIgnoreCase));
            await collectionMatchProcessor!.ProcessAsync().ConfigureAwait(false);
            Console.WriteLine("Collection match data analysing complete");

            Console.WriteLine("Processing collection data..");
            var collectionProcessor = _dataProcessors.FirstOrDefault(processor => processor.DataType.Equals(Keys.COLLECTION, StringComparison.OrdinalIgnoreCase));
            await collectionProcessor!.ProcessAsync().ConfigureAwait(false);
            Console.WriteLine("Collection processing complete");
        }
    }
}
