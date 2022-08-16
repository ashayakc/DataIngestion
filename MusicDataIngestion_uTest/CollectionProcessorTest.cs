using Elasticsearch.Net;
using MusicDataIngestion;
using MusicDataIngestion.Models;
using MusicDataIngestion.Processor;
using Nest;
using Newtonsoft.Json;
using System.Text;

namespace MusicDataIngestion_uTest
{
    public class CollectionProcessorTest
    {
        private readonly IElasticClient _mockElasticClient;
        public CollectionProcessorTest()
        {
            var response = new
            {
                took = 1,
                timed_out = false,
                _shards = new
                {
                    total = 2,
                    successful = 2,
                    failed = 0
                },
                hits = new
                {
                    total = new { value = 25 },
                    max_score = 1.0,
                    hits = Enumerable.Range(1, 25).Select(i => (object)new
                    {
                        _index = "project",
                        _type = "project",
                        _id = $"Project {i}",
                        _score = 1.0,
                        _source = new { name = $"Project {i}" }
                    }).ToArray()
                }
            };
            var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            var connection = new InMemoryConnection(responseBytes, 200);
            var connectionPool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
            var settings = new ConnectionSettings(connectionPool, connection)
                                .DefaultIndex(Settings.IndexName)
                                .PrettyJson()
                                .DisableDirectStreaming()
                                .OnRequestCompleted(response => { });
            _mockElasticClient = new ElasticClient(settings);
        }

        [Fact]
        public async Task ProcessAsync_ShouldAnalyseAndProcessCollection()
        {
            CollectionStore.ArtistCollections.Add(1439161681, new List<long> { 100 });
            CollectionStore.CollectionMatches.Add(1439161681, "UPC");
            CollectionStore.Artists.Add(100, "Taylor swift");
            var processor = new CollectionProcessor(_mockElasticClient, @"D:\Learning\Core\MusicCollection\DataIngestion\MusicDataIngestion_uTest\Dataset\collection", 4);
            var result = await processor.ProcessAsync().ConfigureAwait(false);
            Assert.True(result);
        }
    }
}