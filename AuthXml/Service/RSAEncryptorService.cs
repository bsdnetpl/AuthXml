using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.OpenSsl;
using System.IO;

namespace AuthXml.Service
    {
    public class RSAEncryptorService : IRSAEncryptorService
        {
        public string EncryptText(string text, string publicKeyPath, string timestamp)
            {
            using (var rsa = RSA.Create())
                {
                // Wczytaj klucz publiczny z pliku PEM
                var publicKeyPem = File.ReadAllText(publicKeyPath);

                // Usuń nagłówki i stopki z klucza PEM
                var publicKeyFormatted = publicKeyPem.Replace("-----BEGIN PUBLIC KEY-----", "")
                                                     .Replace("-----END PUBLIC KEY-----", "")
                                                     .Replace("\n", "")
                                                     .Replace("\r", "");

                // Konwertuj klucz do formatu akceptowanego przez RSA
                var keyBytes = Convert.FromBase64String(publicKeyFormatted);
                rsa.ImportSubjectPublicKeyInfo(keyBytes, out _);

                // Dołączamy znacznik czasu do danych do zaszyfrowania
                var dataToEncrypt = Encoding.UTF8.GetBytes($"{timestamp}|{text}");
                var encryptedData = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.Pkcs1);

                // Konwersja zaszyfrowanych danych do formatu Base64
                return Convert.ToBase64String(encryptedData);
                }
            }

        public string DecryptText(string encryptedText, string privateKeyPath, string timestamp)
            {
            // Wczytaj klucz prywatny z pliku PEM
            var privateKeyPem = File.ReadAllText(privateKeyPath);

            // Odczyt klucza prywatnego z użyciem BouncyCastle
            AsymmetricKeyParameter keyParameter;
            using (var reader = new StringReader(privateKeyPem))
                {
                var pemReader = new PemReader(reader);
                var keyObject = pemReader.ReadObject();

                if (keyObject is AsymmetricCipherKeyPair keyPair)
                    {
                    // Gdy klucz zawiera zarówno klucz prywatny, jak i publiczny
                    keyParameter = keyPair.Private;
                    }
                else if (keyObject is RsaPrivateCrtKeyParameters privateKey)
                    {
                    // Gdy obiekt jest samym kluczem prywatnym
                    keyParameter = privateKey;
                    }
                else
                    {
                    throw new InvalidCastException("Nieoczekiwany format klucza prywatnego.");
                    }
                }

            // Konwertuj klucz prywatny na RSAParameters i załaduj go do RSA
            var rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)keyParameter);
            using (var rsa = RSA.Create())
                {
                rsa.ImportParameters(rsaParams);

                // Dekodowanie zaszyfrowanego tekstu z Base64
                byte[] dataToDecrypt = Convert.FromBase64String(encryptedText);

                // Odszyfrowanie tekstu
                byte[] decryptedData = rsa.Decrypt(dataToDecrypt, RSAEncryptionPadding.Pkcs1);

                // Konwersja bajtów na tekst UTF-8
                var decryptedTextWithTimestamp = Encoding.UTF8.GetString(decryptedData);

                // Rozdziel znacznik czasu i oryginalny tekst
                var parts = decryptedTextWithTimestamp.Split('|');
                if (parts.Length != 2)
                    {
                    throw new InvalidOperationException("Nieprawidłowy format zaszyfrowanego tekstu.");
                    }

                var decryptedTimestamp = parts[0];
                var originalText = parts[1];

                // Weryfikacja zgodności przekazanego timestampu
                if (decryptedTimestamp != timestamp)
                    {
                    throw new InvalidOperationException("Znacznik czasu nie jest zgodny. Deszyfrowanie niemożliwe.");
                    }

                // Zwróć oryginalny tekst
                return originalText;
                }
            }
        }
    }
