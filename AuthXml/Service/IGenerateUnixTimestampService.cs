namespace AuthXml.Service
    {
    public interface IGenerateUnixTimestampService
        {
        long GenerateUnixTimestampMilliseconds();
        long GetUnixTimestamp();
        }
    }