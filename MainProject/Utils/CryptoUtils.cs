using System;
using System.Security.Cryptography;
using System.Text;
using BasicDotnetTemplate.MainProject.Models.Settings;

namespace BasicDotnetTemplate.MainProject.Utils;
public class CryptUtils
{
    private readonly string secretKey;
    private const int M = 16;
    private const int N = 32;
    private readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    public CryptUtils(AppSettings appSettings)
    {
        secretKey = appSettings.EncryptionSettings?.Salt ?? String.Empty;
    }

    public string Decrypt(string encryptedData)
    {
        var decrypted = String.Empty;

        if (String.IsNullOrEmpty(this.secretKey) || this.secretKey.Length < M)
        {
            throw new ArgumentException("Unable to proceed with decryption due to invalid settings");
        }

        if (!String.IsNullOrEmpty(encryptedData) && encryptedData.Length > N)
        {
            var iv = encryptedData.Substring(0, M);

            var cipherText = encryptedData.Substring(N);
            var fullCipher = Convert.FromBase64String(cipherText);

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(this.secretKey);
                aes.IV = Encoding.UTF8.GetBytes(iv);

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (var msDecrypt = new MemoryStream(fullCipher))
                    {
                        using (var cryptoStream = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(cryptoStream))
                            {
                                decrypted = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        return decrypted;
    }

}

