using MusicDataIngestion.Constants;
using MusicDataIngestion.Models;

namespace MusicDataIngestion.Processor
{
    public class ArtistProcessor : IDataProcessor
    {
        private readonly string _artistDataFolderPath;
        public ArtistProcessor(string folderPath)
        {
            _artistDataFolderPath = folderPath;
        }

        public string DataType { get => Keys.ARTIST; }

        public async Task<bool> ProcessAsync()
        {
            using var artistReader = new StreamReader(_artistDataFolderPath);
            string artistLine;
            while ((artistLine = await artistReader.ReadLineAsync().ConfigureAwait(false)) != null)
            {
                if (artistLine.StartsWith('#'))
                    continue;

                var columns = artistLine.Split('\u0001');
                //ArtistId, Name
                CollectionStore.Artists.Add(long.Parse(columns[1]), columns[2]);
            }
            return true;
        }
    }
}
