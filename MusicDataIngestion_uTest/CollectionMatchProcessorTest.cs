using Moq;
using MusicDataIngestion.Models;
using MusicDataIngestion.Processor;
using Nest;

namespace MusicDataIngestion_uTest
{
    public class CollectionMatchProcessorTest
    {
        private readonly Mock<IElasticClient> mockElasticClient;
        public CollectionMatchProcessorTest()
        {
            mockElasticClient = new Mock<IElasticClient>();
        }

        [Fact]
        public async Task ProcessAsync_ShouldAnalyseAndProcessCollectionMatch()
        {
            var processor = new CollectionMatchProcessor(@"D:\Learning\Core\MusicCollection\DataIngestion\MusicDataIngestion_uTest\Dataset\collection_match");
            await processor.ProcessAsync(mockElasticClient.Object).ConfigureAwait(false);
            Assert.Equal(7, CollectionStore.CollectionMatches.Count);
        }
    }
}