namespace ArtistDataIngestion.Models
{
    internal class MusicCollection
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string UPC { get; set; }
        public string ReleaseDate { get; set; }
        public bool IsCompilation { get; set; }
        public string Lebel { get; set; }
        public string ImageUrl { get; set; }
        public List<Artist> Artists { get; set; }
    }
}
