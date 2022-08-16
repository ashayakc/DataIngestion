namespace MusicDataIngestion
{
    internal enum ExitCode
    {
        Success = 0,
        PartiallySucceeded = -1,
        InvalidInputParameters = -2,
        Failed = -3
    }
}
