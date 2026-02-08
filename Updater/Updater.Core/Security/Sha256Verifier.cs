using System.Security.Cryptography;

namespace Updater.Core.Security;

public sealed class Sha256Verifier
{
    public static string Compute(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException(
                "File not found for hashing.", filePath);

        using var stream = File.OpenRead(filePath);
        using var sha = SHA256.Create();

        var hash = sha.ComputeHash(stream);

        return Convert.ToHexString(hash);
    }

    public static bool Verify(string filePath, string expectedHash)
    {
        if (string.IsNullOrWhiteSpace(expectedHash))
            throw new ArgumentException("Expected hash is empty.");

        var actual = Compute(filePath);

        return string.Equals(
            actual.ToLower(),
            expectedHash.ToLower(),
            StringComparison.OrdinalIgnoreCase);
    }
}

