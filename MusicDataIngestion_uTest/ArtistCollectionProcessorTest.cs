using MusicDataIngestion.Models;
using MusicDataIngestion.Processor;

namespace MusicDataIngestion_uTest
{
    public class ArtistCollectionProcessorTest
    {
        [Fact]
        public async Task ProcessAsync_ShouldAnalyseAndProcessArtistCollection()
        {
            var processor = new ArtistCollectionProcessor(@"D:\Learning\Core\MusicCollection\DataIngestion\MusicDataIngestion_uTest\Dataset\artist_collection");
            var result = await processor.ProcessAsync().ConfigureAwait(false);
            Assert.True(result);
            Assert.Equal(6, CollectionStore.ArtistCollections.Count);
        }
    }
}