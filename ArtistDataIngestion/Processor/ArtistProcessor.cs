using ArtistDataIngestion.Constants;
using ArtistDataIngestion.Models;
using Nest;

namespace ArtistDataIngestion.Processor
{
    internal class ArtistProcessor : IDataProcessor
    {
        public string DataType { get => Keys.ARTIST; }

        public async Task ProcessAsync(IElasticClient elasticClient)
        {
            using var artistReader = new StreamReader($@"{Settings.ArtistDataFolderPath}");
            string artistLine;
            while ((artistLine = await artistReader.ReadLineAsync().ConfigureAwait(false)) != null)
            {
                if (artistLine.StartsWith('#'))
                    continue;

                var columns = artistLine.Split('\u0001');
                //ArtistId, Name
                CollectionStore.Artists.Add(long.Parse(columns[1]), columns[2]);
            }
        }
    }
}
