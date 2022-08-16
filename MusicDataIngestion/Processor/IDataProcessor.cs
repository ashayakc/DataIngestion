using Nest;

namespace ArtistDataIngestion.Processor
{
    internal interface IDataProcessor
    {
        public string DataType { get; }
        Task ProcessAsync(IElasticClient elasticClient);
    }
}
