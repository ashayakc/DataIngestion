using ArtistDataIngestion.Constants;
using ArtistDataIngestion.Models;
using Nest;

namespace ArtistDataIngestion.Processor
{
    internal class ArtistCollectionProcessor : IDataProcessor
    {
        public string DataType { get => Keys.ARTIST_COLLECTION; }

        public async Task ProcessAsync(IElasticClient elasticClient)
        {
            using var artistCollectionReader = new StreamReader($@"{Settings.ArtistCollectionDataFolderPath}");
            string artistCollectionLine;
            while ((artistCollectionLine = await artistCollectionReader.ReadLineAsync().ConfigureAwait(false)) != null)
            {
                if (artistCollectionLine.StartsWith('#'))
                    continue;

                var columns = artistCollectionLine.Split('\u0001');
                var artistId = long.Parse(columns[1]);
                var collectionId = long.Parse(columns[2]);
                if (CollectionStore.ArtistCollections.ContainsKey(collectionId))
                {
                    var existing = CollectionStore.ArtistCollections[collectionId];
                    existing.Add(artistId);
                    CollectionStore.ArtistCollections[collectionId] = existing;
                    continue;
                }
                CollectionStore.ArtistCollections.Add(collectionId, new List<long> { artistId });
            }
        }
    }
}
