using Nest;

namespace MusicDataIngestion.Processor
{
    public interface IDataProcessor
    {
        public string DataType { get; }
        Task ProcessAsync(IElasticClient elasticClient);
    }
}
