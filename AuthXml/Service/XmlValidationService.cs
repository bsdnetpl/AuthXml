using System.Xml.Linq;
using System.Xml.Schema;

namespace AuthXml.Service
    {
    public class XmlValidationService : IXmlValidationService
        {
        private readonly string _schemaPath = "/xml/schema.xsd";

        public async Task<(bool IsValid, string Errors, object? Data)> ValidateAndProcessXmlAsync(Stream xmlStream)
            {
            try
                {
                // Wczytanie schemy XSD
                var schemaSet = new XmlSchemaSet();
                schemaSet.Add("", _schemaPath);

                // Wczytanie pliku XML
                var xmlDoc = await Task.Run(() => XDocument.Load(xmlStream));

                // Walidacja XML względem schemy XSD
                string validationErrors = string.Empty;
                xmlDoc.Validate(schemaSet, (o, e) =>
                {
                    validationErrors += $"{e.Message}\n";
                });

                if (!string.IsNullOrEmpty(validationErrors))
                    {
                    return (false, validationErrors, null);
                    }

                // Przetwarzanie poprawnych danych XML
                var data = ProcessXmlData(xmlDoc);
                return (true, string.Empty, data);
                }
            catch (Exception ex)
                {
                return (false, ex.Message, null);
                }
            }

        private object ProcessXmlData(XDocument xmlDoc)
            {
            var root = xmlDoc.Root;
            if (root == null || root.Name != "Request")
                {
                throw new Exception("Niepoprawny główny element XML.");
                }

            var timestamp = (DateTime?)root.Element("Timestamp");
            var nip = (string)root.Element("NIP");
            var encryptedToken = (string)root.Element("EncryptedToken");

            return new
                {
                Timestamp = timestamp,
                NIP = nip,
                EncryptedToken = encryptedToken
                };
            }
        }
    }
