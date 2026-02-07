using System.Net.Http.Json;
using Updater.Core.Abstractions;
using Updater.Core.Models;

namespace Updater.Core.Services;

public sealed class PackageClient : IPackageClient
{

    private static readonly HttpClient client = new();

    public async Task<Manifest> FetchingManifestAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("ManifestUrl cannot be empty.");
        }

        var manifest =
            await client.GetFromJsonAsync<Manifest>(url);

        if (manifest == null)
            throw new InvalidOperationException("The manifesto could not be read.");

        return manifest;
    }

    public async Task<string> DownloadAsync(string url, string targetDirectory, IProgress<int>? progress = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Download URL is empty.");

        Directory.CreateDirectory(targetDirectory);

        var fileName = Path.GetFileName(new Uri(url).LocalPath);

        if (string.IsNullOrWhiteSpace(fileName))
            fileName = "update_package.enc";

        var targetPath =
            Path.Combine(targetDirectory, fileName);

        using var response =
            await client.GetAsync(
                url,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);

        response.EnsureSuccessStatusCode();

        var totalBytes =
            response.Content.Headers.ContentLength;

        using var input =
            await response.Content.ReadAsStreamAsync(cancellationToken);

        using var output =
            new FileStream(
                targetPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                8192,
                true);

        var buffer = new byte[8192];

        long totalRead = 0;
        int read;

        while ((read = await input.ReadAsync(
                   buffer, cancellationToken)) > 0)
        {
            await output.WriteAsync(
                buffer.AsMemory(0, read),
                cancellationToken);

            totalRead += read;

            if (totalBytes.HasValue && progress != null)
            {
                var percent =
                    (int)((totalRead * 100) / totalBytes.Value);

                progress.Report(percent);
            }
        }

        return targetPath;
    }

  
}
