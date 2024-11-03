using System.Security.Cryptography;
using System.Text;

namespace AuthXml.Service
    {
    public class GenerateTokenService: IGenerateTokenService
        {
        public string GenerateToken(int length = 32)
            {
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var token = new StringBuilder(length);
            using (var rng = new RNGCryptoServiceProvider())
                {
                var byteBuffer = new byte[sizeof(uint)];

                while (token.Length < length)
                    {
                    rng.GetBytes(byteBuffer);
                    uint randomInt = BitConverter.ToUInt32(byteBuffer, 0);
                    token.Append(allowedChars[(int)(randomInt % (uint)allowedChars.Length)]);
                    }
                }

            return token.ToString();
            }
        }
    }
