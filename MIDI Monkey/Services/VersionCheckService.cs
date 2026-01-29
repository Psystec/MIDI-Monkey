using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MIDI_Monkey.Models;
using MIDI_Monkey.Utilities;

namespace MIDI_Monkey.Services
{
    public class VersionCheckService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string? _cachedVersion;
        private static string? _lastETag;

        public async Task<bool> CheckForUpdatesAsync(string currentVersion)
        {
            try
            {
                Logging.DebugLog("Checking for updates...");
                var latestVersion = await GetLatestReleaseVersionAsync();
                Logging.DebugLog($"Latest version: {latestVersion}");

                int comparison = CompareVersions(currentVersion, latestVersion);

                if (comparison < 0)
                {
                    Logging.DebugLog($"A new version of MIDI Monkey is available!\nPlease update.\nLink: https://github.com/Psystec/MIDI-Monkey/releases/latest");
                    return true;
                }
                else if (comparison > 0)
                {
                    Logging.DebugLog($"Running development version ({currentVersion}). Latest stable release: {latestVersion}");
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
                Logging.DebugLog($"Error checking for updates: {ex.Message}");
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

        private async Task<string> GetLatestReleaseVersionAsync()
        {
            string url = "https://api.github.com/repos/Psystec/MIDI-Monkey/releases/latest";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "MIDI-Monkey-Version-Checker/1.0");

            if (!string.IsNullOrEmpty(_lastETag))
            {
                _httpClient.DefaultRequestHeaders.Add("If-None-Match", _lastETag);
            }

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.NotModified)
            {
                return _cachedVersion ?? "No version found";
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"GitHub API request failed: {response.StatusCode} - {response.ReasonPhrase}");
            }

            if (response.Headers.ETag != null)
            {
                _lastETag = response.Headers.ETag.Tag;
            }

            string jsonContent = await response.Content.ReadAsStringAsync();
            var release = JsonSerializer.Deserialize<GitHubRelease>(jsonContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });

            _cachedVersion = release?.TagName ?? "No version found";

            return _cachedVersion;
        }
    }
}
