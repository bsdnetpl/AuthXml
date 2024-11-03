using System.Xml.Schema;
using System.Xml;

namespace AuthXml.Service
    {
    public class ParseAndValidateXmlService : IParseAndValidateXmlService
        {
        public void ParseAndValidateXml(string xmlFilePath, string xsdFilePath)
            {
            // Ustawienia do walidacji XML
            var settings = new XmlReaderSettings();
            settings.Schemas.Add(null, xsdFilePath);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += ValidationEventHandler;

            // Parsowanie XML i walidacja z XSD
            using (var reader = XmlReader.Create(xmlFilePath, settings))
                {
                // Przemieszczanie się przez węzły XML
                while (reader.Read())
                    {
                    if (reader.NodeType == XmlNodeType.Element)
                        {
                        switch (reader.Name)
                            {
                            case "Timestamp":
                                reader.Read();
                                Console.WriteLine("Timestamp: " + reader.Value);
                                break;

                            case "NIP":
                                reader.Read();
                                Console.WriteLine("NIP: " + reader.Value);
                                break;

                            case "EncryptedToken":
                                reader.Read();
                                Console.WriteLine("EncryptedToken: " + reader.Value);
                                break;
                            }
                        }
                    }
                }
            }
        private void ValidationEventHandler(object sender, ValidationEventArgs e)
            {
            if (e.Severity == XmlSeverityType.Warning)
                Console.WriteLine("Ostrzeżenie walidacji: " + e.Message);
            else if (e.Severity == XmlSeverityType.Error)
                Console.WriteLine("Błąd walidacji: " + e.Message);
            }
        }

    }
    
