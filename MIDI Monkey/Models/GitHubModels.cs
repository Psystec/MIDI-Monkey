using System;

namespace MIDI_Monkey.Models
{
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
