using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace MIDI_Monkey
{
    public class GitHubVersionChecker
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public async Task<bool> Check(string appVersion)
        {
            try
            {
                Common.ShowErrorMessage($"Checking Version...", false);
                var version = await GetLatestReleaseVersionAsync();
                Common.ShowErrorMessage($"Latest version: {version}", false);

                if (version != appVersion)
                {
                    Common.ShowErrorMessage($"A new version of MIDI Monkey is availabe!\nPlease update.\nLink: https://github.com/Psystec/MIDI-Monkey/releases/latest", false);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Common.ShowErrorMessage(ex.Message, true);
                return false;
            }
        }

        public static async Task<string> GetLatestReleaseVersionAsync()
        {
            string url = $"https://api.github.com/repos/Psystec/MIDI-Monkey/releases/latest";
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "GitHubVersionChecker/1.0");

            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"GitHub API request failed: {response.StatusCode} - {response.ReasonPhrase}");
            }

            string jsonContent = await response.Content.ReadAsStringAsync();
            var release = JsonSerializer.Deserialize<GitHubRelease>(jsonContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });

            return release?.TagName ?? "No version found";
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