using System.Security.Cryptography;
using System.Text;

namespace Updater.Core.Security;

public sealed class AesCryptor
{
   private static byte[] Salt => Encoding.UTF8.GetBytes("AutoUpdater-Salt");

    public static async Task EncryptAsync(
      string inputFile,
      string licenseKey,
      string outputFile)
    {
        if (!File.Exists(inputFile))
            throw new FileNotFoundException(inputFile);


        using var derive =
            new Rfc2898DeriveBytes(
                licenseKey,
                Salt,
                100_000,
                HashAlgorithmName.SHA256);

        var key = derive.GetBytes(32);
        var iv = derive.GetBytes(16);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        await using var input =
            new FileStream(inputFile, FileMode.Open);

        await using var output =
            new FileStream(outputFile, FileMode.Create);

        await using var crypto =
            new CryptoStream(
                output,
                aes.CreateEncryptor(),
                CryptoStreamMode.Write);

        await input.CopyToAsync(crypto);
    }
    public static async Task<string> DecryptAsync(
     string encryptedFile,
     string licenseKey,
     string outputDir)
    {
        if (!File.Exists(encryptedFile))
            throw new FileNotFoundException(
                "Encrypted package not found.");

        if (string.IsNullOrWhiteSpace(licenseKey))
            throw new ArgumentException("License key is empty.");

        Directory.CreateDirectory(outputDir);

        var outputPath =
            Path.Combine(
                outputDir,
                Path.GetFileNameWithoutExtension(encryptedFile) + ".exe");


        using var derive =
            new Rfc2898DeriveBytes(
                licenseKey,
                Salt,
                100_000,
                HashAlgorithmName.SHA256);

        var key = derive.GetBytes(32);
        var iv = derive.GetBytes(16);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        await using var input =
            new FileStream(encryptedFile, FileMode.Open);

        await using var output =
            new FileStream(outputPath, FileMode.Create);

        await using var crypto =
            new CryptoStream(
                input,
                aes.CreateDecryptor(),
                CryptoStreamMode.Read);

        await crypto.CopyToAsync(output);

        return outputPath;
    }
}
