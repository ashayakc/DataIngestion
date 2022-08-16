using MusicDataIngestion.Models;
using MusicDataIngestion.Processor;

namespace MusicDataIngestion_uTest
{
    public class ArtistProcessorTest
    {
        [Fact]
        public async Task ProcessAsync_ShouldAnalyseAndProcessCollectionMatch()
        {
            var processor = new ArtistProcessor(@"D:\Learning\Core\MusicCollection\DataIngestion\MusicDataIngestion_uTest\Dataset\artist");
            var result = await processor.ProcessAsync().ConfigureAwait(false);
            Assert.True(result);
            Assert.Equal(7, CollectionStore.Artists.Count);
        }
    }
}