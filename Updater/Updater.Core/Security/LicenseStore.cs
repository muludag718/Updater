using System.Security.Cryptography;
using System.Text;
using Updater.Core.Models;

namespace Updater.Core.Security;

public static class LicenseStore
{
    private const string FileName = "license.dat";

    private static string GetPath()
    {

        var dir = UpdateConfig.Instance.LocalRootPath;

        Directory.CreateDirectory(dir);

        return Path.Combine(dir, FileName);
    }

    private static void EnsureWindows()
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException(
                "License protection is supported only on Windows.");
        }
    }

    public static void Save(string licenseKey)
    {
        EnsureWindows();

        if (string.IsNullOrWhiteSpace(licenseKey))
            throw new ArgumentException("License key is empty.");

        var bytes =
            Encoding.UTF8.GetBytes(licenseKey);

#pragma warning disable CA1416
        var encrypted =
            ProtectedData.Protect(
                bytes,
                null,
                DataProtectionScope.CurrentUser);

        File.WriteAllBytes(GetPath(), encrypted);
    }

    public static string Load()
    {
        EnsureWindows();

        var path = GetPath();

        if (!File.Exists(path))
            throw new FileNotFoundException("License not found.");

        var encrypted = File.ReadAllBytes(path);

        var decrypted =
            ProtectedData.Unprotect(
                encrypted,
                null,
                DataProtectionScope.CurrentUser);

        return Encoding.UTF8.GetString(decrypted);
    }

    public static bool Exists()
    {
        return File.Exists(GetPath());
    }
}
