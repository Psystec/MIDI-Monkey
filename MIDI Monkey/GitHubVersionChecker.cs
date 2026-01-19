using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace MIDI_Monkey
{
    public class GitHubVersionChecker
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static string? cachedVersion;
        private static string? lastETag;

        public async Task<bool> Check(string appVersion)
        {
            try
            {
                Logging.DebugLog("Checking Version...");
                var version = await GetLatestReleaseVersionAsync();
                Logging.DebugLog($"Latest version: {version}");

                int comparison = CompareVersions(appVersion, version);

                if (comparison < 0)
                {
                    Logging.DebugLog($"A new version of MIDI Monkey is available!\nPlease update.\nLink: https://github.com/Psystec/MIDI-Monkey/releases/latest");
                    return true;
                }
                else if (comparison > 0)
                {
                    Logging.DebugLog($"Running development version ({appVersion}). Latest stable release: {version}");
                    return false;
                }
                else
                {
                    Logging.DebugLog($"MIDI Monkey is up to date!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Common.ShowErrorMessage(ex.Message, true);
                return false;
            }
        }

        private int CompareVersions(string current, string latest)
        {
            current = current.TrimStart('v', 'V');
            latest = latest.TrimStart('v', 'V');

            try
            {
                Version currentVersion = new Version(current);
                Version latestVersion = new Version(latest);
                return currentVersion.CompareTo(latestVersion);
            }
            catch
            {
                return string.Compare(current, latest, StringComparison.OrdinalIgnoreCase);
            }
        }

        public static async Task<string> GetLatestReleaseVersionAsync()
        {
            string url = "https://api.github.com/repos/Psystec/MIDI-Monkey/releases/latest";
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "MIDI-Monkey-Version-Checker/1.0");

            if (!string.IsNullOrEmpty(lastETag))
            {
                httpClient.DefaultRequestHeaders.Add("If-None-Match", lastETag);
            }

            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.NotModified)
            {
                return cachedVersion ?? "No version found";
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"GitHub API request failed: {response.StatusCode} - {response.ReasonPhrase}");
            }

            if (response.Headers.ETag != null)
            {
                lastETag = response.Headers.ETag.Tag;
            }

            string jsonContent = await response.Content.ReadAsStringAsync();
            var release = JsonSerializer.Deserialize<GitHubRelease>(jsonContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });

            cachedVersion = release?.TagName ?? "No version found";

            return cachedVersion;
        }
    }

    public class GitHubRelease
    {
        public string TagName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; }
        public bool Prerelease { get; set; }
        public bool Draft { get; set; }
        public string Body { get; set; } = string.Empty;
    }

    public class GitHubTag
    {
        public string Name { get; set; } = string.Empty;
        public GitHubCommit Commit { get; set; } = new GitHubCommit();
    }

    public class GitHubCommit
    {
        public string Sha { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}