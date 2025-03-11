using System;
using System.Security.Cryptography;
using System.Text;
using BasicDotnetTemplate.MainProject.Models.Settings;

namespace BasicDotnetTemplate.MainProject.Utils;
public class CryptUtils
{
    private readonly string _secretKey;
    private readonly string _pepper;
    private const int _M = 16;
    private const int _N = 32;

    public CryptUtils(AppSettings appSettings)
    {
        _secretKey = appSettings.EncryptionSettings?.Salt ?? String.Empty;
        _pepper = appSettings.EncryptionSettings?.Pepper ?? String.Empty;
    }

    public string Decrypt(string encryptedData)
    {
        var decrypted = String.Empty;

        if (String.IsNullOrEmpty(this._secretKey) || this._secretKey.Length < _M)
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
                aes.Key = Encoding.UTF8.GetBytes(this._secretKey);
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

    public static string GenerateSalt()
    {
        using var rng = RandomNumberGenerator.Create();
        var byteSalt = new byte[16];
        rng.GetBytes(byteSalt);
        var salt = Convert.ToBase64String(byteSalt);
        return salt;
    }

    public string GeneratePassword(string password, string salt, int iteration)
    {
        string hashedPassword = password;
        for(var i = 0; i <= iteration; i++)
        {
            using var sha256 = SHA256.Create();
            var passwordSaltPepper = $"{hashedPassword}{salt}{this._pepper}";
            var byteValue = Encoding.UTF8.GetBytes(passwordSaltPepper);
            var byteHash = sha256.ComputeHash(byteValue);
            hashedPassword = Convert.ToBase64String(byteHash);
        }

        return hashedPassword;
    }

    public bool VerifyPassword(string password, string salt, int iteration, string userPassword)
    {
        string hashedPassword = this.GeneratePassword(password, salt, iteration);
        return hashedPassword.Equals(userPassword, StringComparison.OrdinalIgnoreCase);
    }



}

