using Microsoft.Extensions.Configuration;

namespace ArtistDataIngestion
{
    internal class Settings
    {
        private static IConfiguration Config => new ConfigurationBuilder()
                .AddJsonFile("appSettings.json", false, true)
                .Build();

        public static string ElasticUrl => Config["ElasticUrl"];
        public static string IndexName => Config["IndexName"];
        public static string ElasticUserName => Config["ElasticUserName"];
        public static string ElasticPassword => Config["ElasticPassword"];
        public static string ArtistDataFolderPath => Config["ArtistDataFolderPath"];
        public static string ArtistCollectionDataFolderPath => Config["ArtistCollectionDataFolderPath"];
        public static string CollectionMatchDataFolderPath => Config["CollectionMatchDataFolderPath"];
        public static string CollectionDataFolderPath => Config["CollectionDataFolderPath"];
        public static int BatchLimit => int.Parse(Config["BatchLimit"]);
    }
}
