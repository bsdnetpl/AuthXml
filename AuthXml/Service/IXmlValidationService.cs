
namespace AuthXml.Service
    {
    public interface IXmlValidationService
        {
        Task<(bool IsValid, string Errors, object? Data)> ValidateAndProcessXmlAsync(Stream xmlStream);
        }
    }