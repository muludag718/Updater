using System.Text.Json;
using System.Text.Json.Serialization;

namespace Updater.Core.Models;

public sealed class Manifest
{
    [JsonPropertyName("appName")]
    public string AppName { get; set; } = "";

    [JsonPropertyName("version")]
    public string Version { get; set; } = "1.0.0";

    [JsonPropertyName("minSupportedVersion")]
    public string MinSupportedVersion { get; set; } = "1.0.0";

    [JsonPropertyName("mandatory")]
    public bool Mandatory { get; set; }

    // Full URL to encrypted package (.enc)
    [JsonPropertyName("asset")]
    public string Asset { get; set; } = "";

    [JsonPropertyName("hashSha256")]
    public string HashSha256 { get; set; } = "";

    // Extra msiexec args (optional)
    [JsonPropertyName("silentArgs")]
    public string SilentArgs { get; set; } = "";

    [JsonPropertyName("releaseNotes")]
    public string ReleaseNotes { get; set; } = "";
}
