using MusicDataIngestion.Constants;
using MusicDataIngestion.Models;

namespace MusicDataIngestion.Processor
{
    public class CollectionMatchProcessor : IDataProcessor
    {
        private readonly string _artistDataFolderPath;
        public CollectionMatchProcessor(string folderPath)
        {
            _artistDataFolderPath = folderPath;
        }

        public string DataType { get => Keys.COLLECTION_MATCH; }

        public async Task<bool> ProcessAsync()
        {
            using var collectionMatchReader = new StreamReader(_artistDataFolderPath);
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
            return true;
        }
    }
}
