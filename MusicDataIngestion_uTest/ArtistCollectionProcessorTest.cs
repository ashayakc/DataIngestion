using Microsoft.Extensions.Configuration;
using Moq;
using MusicDataIngestion.Models;
using MusicDataIngestion.Processor;
using Nest;

namespace MusicDataIngestion_uTest
{
    public class ArtistCollectionProcessorTest
    {
        private readonly Mock<IElasticClient> mockElasticClient;
        public ArtistCollectionProcessorTest()
        {
            mockElasticClient = new Mock<IElasticClient>();
        }

        [Fact]
        public async Task ProcessAsync_ShouldAnalyseAndProcessArtistCollection()
        {
            var processor = new ArtistCollectionProcessor(@"D:\Learning\Core\MusicCollection\DataIngestion\MusicDataIngestion_uTest\Dataset\artist_collection");
            await processor.ProcessAsync(mockElasticClient.Object).ConfigureAwait(false);
            Assert.Equal(6, CollectionStore.ArtistCollections.Count);
        }
    }
}