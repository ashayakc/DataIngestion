using ArtistDataIngestion.Constants;
using ArtistDataIngestion.Models;
using Nest;

namespace ArtistDataIngestion.Processor
{
    internal class CollectionProcessor : IDataProcessor
    {
        public string DataType { get => Keys.COLLECTION; }

        public async Task ProcessAsync(IElasticClient elasticClient)
        {
            var counter = 1;
            long collectionId = 0;
            await foreach (var batch in ReadBatchesAsync($@"{Settings.CollectionDataFolderPath}"))
            {
                var musicCollection = new List<MusicCollection>();
                Console.WriteLine($"*** Processing Batch - {counter} ***");

                foreach (var collectionLine in batch)
                {
                    try
                    {
                        if (collectionLine.StartsWith('#'))
                            continue;

                        musicCollection.Add(GetMusicCollection(collectionLine));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Something went wrong when processing collection {collectionId} with message: {ex.Message}");
                    }
                }

                elasticClient.BulkAll(musicCollection, x => x
                                        .Index(Settings.IndexName)
                                        .BackOffTime("30s")
                                        .BackOffRetries(2)
                                        .RefreshOnCompleted()
                                        .ContinueAfterDroppedDocuments()
                                        .MaxDegreeOfParallelism(Environment.ProcessorCount)
                                        .Size(Settings.BatchLimit)
                                    )
                                    .Wait(TimeSpan.FromMinutes(15), next =>
                                    {
                                        Console.WriteLine("Batch processed successully to elastic");
                                    });
                musicCollection.Clear();
                counter++;
            }
        }

        private static MusicCollection GetMusicCollection(string collectionLine)
        {
            var columns = collectionLine.Split('\u0001');
            var collectionId = long.Parse(columns[1]);
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

        private static async IAsyncEnumerable<IEnumerable<string>> ReadBatchesAsync(string fileName)
        {
            using var file = File.OpenText(fileName);
            var batchItems = new List<string>();

            while (!file.EndOfStream)
            {
                // clear the batch list
                batchItems.Clear();

                for (int i = 0; i < Settings.BatchLimit; i++)
                {
                    if (file.EndOfStream)
                        break;

                    batchItems.Add((await file.ReadLineAsync().ConfigureAwait(false))!);
                }

                yield return batchItems;
            }

            file.Close();
        }
    }
}