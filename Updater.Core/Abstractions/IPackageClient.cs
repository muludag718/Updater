using Updater.Core.Models;

namespace Updater.Core.Abstractions;

public interface IPackageClient
{
    Task<Manifest> FetchingManifestAsync(string url);
    Task<string> DownloadAsync(
    string url,
    string targetDirectory,
    IProgress<int>? progress = null,
    CancellationToken cancellationToken = default);

  
}
