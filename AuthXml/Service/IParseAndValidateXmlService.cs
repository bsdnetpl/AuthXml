namespace AuthXml.Service
    {
    public interface IParseAndValidateXmlService
        {
        void ParseAndValidateXml(string xmlFilePath, string xsdFilePath);
        }
    }