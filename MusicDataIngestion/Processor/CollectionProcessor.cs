using MusicDataIngestion.Constants;
using MusicDataIngestion.Models;
using Nest;

namespace MusicDataIngestion.Processor
{
    public class CollectionProcessor : IDataProcessor
    {
        private readonly IElasticClient _elasticClient;
        private readonly string _collectionDataFolderPath;
        private readonly int _batchLimit;
        public CollectionProcessor(IElasticClient elasticClient, string folderPath, int batchLimit)
        {
            _elasticClient = elasticClient;
            _collectionDataFolderPath = folderPath;
            _batchLimit = batchLimit;
        }

        public string DataType { get => Keys.COLLECTION; }

        public async Task<bool> ProcessAsync()
        {
            var counter = 1;
            await foreach (var batch in ReadBatchesAsync(_collectionDataFolderPath))
            {
                var musicCollection = new List<MusicCollection>();
                Console.WriteLine($"*** Processing Batch - {counter} ***");

                foreach (var collectionLine in batch)
                {
                    if (collectionLine.StartsWith('#'))
                        continue;

                    var collection = GetMusicCollection(collectionLine);
                    if(collection != null)
                    {
                        musicCollection.Add(collection);
                    }
                }

                _elasticClient.BulkAll(musicCollection, x => x
                                        .Index(Settings.IndexName)
                                        .BackOffTime("30s")
                                        .BackOffRetries(2)
                                        .RefreshOnCompleted()
                                        .ContinueAfterDroppedDocuments()
                                        .MaxDegreeOfParallelism(Environment.ProcessorCount)
                                        .Size(_batchLimit)
                                    )
                                    .Wait(TimeSpan.FromMinutes(15), next =>
                                    {
                                        Console.WriteLine("Batch processed successully to elastic");
                                    });
                musicCollection.Clear();
                counter++;
                GC.Collect();
            }
            return true;
        }

        private static MusicCollection GetMusicCollection(string collectionLine)
        {
            long collectionId = 0;
            try
            {
                var columns = collectionLine.Split('\u0001');
                collectionId = long.Parse(columns[1]);
                var artistIds = CollectionStore.ArtistCollections
                                    .Where(x => x.Key == collectionId)
                                    .SelectMany(x => x.Value)
                                    .Distinct();
                var artists = artistIds.Select(artistId =>
                {
                    return new Artist
                    {
                        Id = artistId,
                        Name = CollectionStore.Artists.ContainsKey(artistId) ? CollectionStore.Artists[artistId] : string.Empty
                    };

                }).ToList();

                return new MusicCollection
                {
                    Id = long.Parse(columns[1]),
                    Name = columns[2],
                    Url = columns[7],
                    ImageUrl = columns[8],
                    ReleaseDate = columns[9],
                    Lebel = columns[11],
                    IsCompilation = bool.TryParse(columns[16], out bool result),
                    UPC = CollectionStore.CollectionMatches[collectionId],
                    Artists = artists
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong when processing collection {collectionId} with message: {ex.Message}");
                return null!;
            }
        }

        private async IAsyncEnumerable<IEnumerable<string>> ReadBatchesAsync(string fileName)
        {
            using var reader = new StreamReader(fileName);
            while (!reader.EndOfStream)
            {
                // clear the batch list
                var batchItems = new List<string>();

                for (int i = 0; i < _batchLimit; i++)
                {
                    if (reader.EndOfStream)
                        break;

                    batchItems.Add((await reader.ReadLineAsync().ConfigureAwait(false))!);
                }
                yield return batchItems;
            }

            reader.Close();
        }
    }
}