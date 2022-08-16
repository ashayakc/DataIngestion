namespace MusicDataIngestion.Models
{
    internal class CollectionStore
    {
        public static Dictionary<long, List<long>> ArtistCollections { get; set; } = new Dictionary<long, List<long>>(); //CollectionId, ArtistId
        public static Dictionary<long, string> Artists { get; set; } = new Dictionary<long, string>(); //ArtistId, Name
        public static Dictionary<long, string> CollectionMatches { get; set; } = new Dictionary<long, string>(); //collectionId, UPC
    }
}
