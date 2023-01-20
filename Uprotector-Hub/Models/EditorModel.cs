using System;
using Newtonsoft.Json;

namespace Uprotector_Hub.Models;

public class EditorModel
{
    [JsonProperty("version")] public string Version { get; set; } = "Unknown";

    [JsonProperty("architecture")] public string Architecture { get; set; } = "Unknown";

    [JsonProperty("location")] public string[] Location { get; set; } = Array.Empty<string>();

    [JsonProperty("buildPlatforms")]
    public EditorBuildPlatform[] BuildPlatforms { get; set; } = Array.Empty<EditorBuildPlatform>();
}

public class EditorBuildPlatform
{
    [JsonProperty("dirName")] public string DirName { get; set; } = "";

    [JsonProperty("name")] public string Name { get; set; } = "";

    [JsonProperty("buildTarget")] public string BuildTarget { get; set; } = "";
}