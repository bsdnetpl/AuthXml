namespace AuthXml.Service
    {
    public interface IGenerateTokenService
        {
        string GenerateToken(int length);
        }
    }