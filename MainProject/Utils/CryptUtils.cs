using System;
using System.Security.Cryptography;
using System.Text;
using BasicDotnetTemplate.MainProject.Models.Settings;

namespace BasicDotnetTemplate.MainProject.Utils;
public class CryptUtils(AppSettings appSettings)
{
    private readonly string _saltKey = appSettings.EncryptionSettings?.SaltKey ?? String.Empty;
    private const int _M = 16;
    private const int _N = 32;

    public string Decrypt(string encryptedData)
    {
        var decrypted = String.Empty;

        if (String.IsNullOrEmpty(this._saltKey) || this._saltKey.Length < _M)
        {
            throw new ArgumentException("Unable to proceed with decryption due to invalid settings");
        }

        if (!String.IsNullOrEmpty(encryptedData) && encryptedData.Length > _N)
        {
            var iv = encryptedData.Substring(0, _M);

            var cipherText = encryptedData.Substring(_N);
            var fullCipher = Convert.FromBase64String(cipherText);

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(this._saltKey);
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

    public static string GeneratePepper()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytePepper = new byte[16];
        rng.GetBytes(bytePepper);
        var pepper = Convert.ToBase64String(bytePepper);
        return pepper;
    }

    public static string GeneratePassword(string password, string salt, int iterations, string? pepper = "")
    {
        string hashedPassword = password;
        for (var i = 0; i <= iterations; i++)
        {
            var passwordSaltPepper = $"{hashedPassword}{salt}{pepper}";
            var byteValue = Encoding.UTF8.GetBytes(passwordSaltPepper);
            var byteHash = SHA256.HashData(byteValue);
            hashedPassword = Convert.ToBase64String(byteHash);
        }

        return hashedPassword;
    }

    public static bool VerifyPassword(string userPassword, string password, string salt, int iterations, string? pepper = "")
    {
        string hashedPassword = GeneratePassword(password, salt, iterations, pepper);
        return hashedPassword.Equals(userPassword, StringComparison.OrdinalIgnoreCase);
    }



}

