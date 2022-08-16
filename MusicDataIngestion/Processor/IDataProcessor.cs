using Nest;

namespace MusicDataIngestion.Processor
{
    internal interface IDataProcessor
    {
        public string DataType { get; }
        Task ProcessAsync(IElasticClient elasticClient);
    }
}
