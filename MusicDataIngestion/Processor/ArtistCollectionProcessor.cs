using MusicDataIngestion.Constants;
using MusicDataIngestion.Models;

namespace MusicDataIngestion.Processor
{
    public class ArtistCollectionProcessor : IDataProcessor
    {
        private readonly string _artistCollectionFolderPath;
        public ArtistCollectionProcessor(string folderPath)
        {
            _artistCollectionFolderPath = folderPath;
        }

        public string DataType { get => Keys.ARTIST_COLLECTION; }

        public async Task<bool> ProcessAsync()
        {
            using var artistCollectionReader = new StreamReader(_artistCollectionFolderPath);
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
            return true;
        }
    }
}
