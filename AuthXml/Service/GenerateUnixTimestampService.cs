namespace AuthXml.Service
    {
    public class GenerateUnixTimestampService : IGenerateUnixTimestampService
        {
        public long GenerateUnixTimestampMilliseconds()
            {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
        public long GetUnixTimestamp()
            {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
            }
        }
    }
    