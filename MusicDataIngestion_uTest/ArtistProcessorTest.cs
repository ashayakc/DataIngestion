using Moq;
using MusicDataIngestion.Models;
using MusicDataIngestion.Processor;
using Nest;

namespace MusicDataIngestion_uTest
{
    public class ArtistProcessorTest
    {
        private readonly Mock<IElasticClient> mockElasticClient;
        public ArtistProcessorTest()
        {
            mockElasticClient = new Mock<IElasticClient>();
        }

        [Fact]
        public async Task ProcessAsync_ShouldAnalyseAndProcessCollectionMatch()
        {
            var processor = new ArtistProcessor(@"D:\Learning\Core\MusicCollection\DataIngestion\MusicDataIngestion_uTest\Dataset\artist");
            await processor.ProcessAsync(mockElasticClient.Object).ConfigureAwait(false);
            Assert.Equal(7, CollectionStore.Artists.Count);
        }
    }
}