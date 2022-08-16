using MusicDataIngestion.Constants;
using MusicDataIngestion.Models;
using Nest;

namespace MusicDataIngestion.Processor
{
    internal class CollectionMatchProcessor : IDataProcessor
    {
        public string DataType { get => Keys.COLLECTION_MATCH; }

        public async Task ProcessAsync(IElasticClient elasticClient)
        {
            using var collectionMatchReader = new StreamReader($@"{Settings.CollectionMatchDataFolderPath}");
            string collectionMatchLine;
            while ((collectionMatchLine = await collectionMatchReader.ReadLineAsync().ConfigureAwait(false)) != null)
            {
                if (collectionMatchLine.StartsWith('#'))
                    continue;

                var columns = collectionMatchLine.Split('\u0001');
                var collectionId = long.Parse(columns[1]);
                if (!CollectionStore.CollectionMatches.ContainsKey(collectionId))
                {
                    CollectionStore.CollectionMatches.Add(collectionId, columns[2]);
                }
            }
        }
    }
}
