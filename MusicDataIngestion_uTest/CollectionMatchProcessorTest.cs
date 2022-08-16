using MusicDataIngestion.Models;
using MusicDataIngestion.Processor;

namespace MusicDataIngestion_uTest
{
    public class CollectionMatchProcessorTest
    {
        [Fact]
        public async Task ProcessAsync_ShouldAnalyseAndProcessCollectionMatch()
        {
            var processor = new CollectionMatchProcessor(@"D:\Learning\Core\MusicCollection\DataIngestion\MusicDataIngestion_uTest\Dataset\collection_match");
            var result = await processor.ProcessAsync().ConfigureAwait(false);
            Assert.True(result);
            Assert.Equal(7, CollectionStore.CollectionMatches.Count);
        }
    }
}