namespace AuthXml.Service
    {
    public interface IGenerateUnixTimestampService
        {
        string GenerateIso8601Timestamp();
        long GenerateUnixTimestampMilliseconds();
        long GetUnixTimestamp();
        }
    }