namespace AuthXml.Service
    {
    public interface IRSAEncryptorService
        {
        string DecryptText(string encryptedText, string privateKeyPath);
        string EncryptText(string text, string publicKeyPath);
        }
    }