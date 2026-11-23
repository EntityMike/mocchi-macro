// The update check: asks GitHub once for the tool's latest release and says whether it's newer
// than the running version, so the window can show the update notice. Any failure, such as
// being offline, a timeout, rate limiting, or a 404, counts as no newer release.

using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MocchiMacro
{
    public static class UpdateCheck
    {
        // /latest skips drafts and pre-releases.
        const string LatestReleaseApiUrl = "https://api.github.com/repos/" + ToolDetails.Repo + "/releases/latest";

        // Where the update notice's link goes, for organizers.
        public const string LatestReleasePageUrl = ToolDetails.RepoPageUrl + "/releases/latest";

        // The release's tag, newer than the running version, or null when there's none.
        // Never throws: an organizer offline or behind a firewall just sees no notice.
        public static async Task<Version> FindNewerReleaseAsync(Version running)
        {
            try
            {
                using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) })
                {
                    // GitHub's API refuses requests without one.
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("MocchiMacro");
                    string body = await client.GetStringAsync(LatestReleaseApiUrl).ConfigureAwait(false);

                    // One field doesn't need a JSON library; the release spec says why.
                    Match tagMatch = Regex.Match(body, "\"tag_name\"\\s*:\\s*\"([^\"]+)\"");
                    return NewerRelease(tagMatch.Success ? tagMatch.Groups[1].Value : null, running);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        // The tag as a three-part version when it's newer than the running one, or null when it
        // isn't, or isn't a version. Nothing is stripped first, so a "v1.1.0" tag counts as no
        // newer release: docs/releasing.md says tags are plain versions.
        public static Version NewerRelease(string tag, Version running)
        {
            if (!Version.TryParse(tag, out Version release)) return null;
            Version newer = ToThreeParts(release);
            return newer > ToThreeParts(running) ? newer : null;
        }

        // Version counts a missing part as lower than 0, so 1.1.0 would be older than 1.1.0.0
        // without this. A two-part tag such as "1.1" gets a 0 build.
        static Version ToThreeParts(Version version)
        {
            return new Version(version.Major, version.Minor, Math.Max(version.Build, 0));
        }
    }
}
