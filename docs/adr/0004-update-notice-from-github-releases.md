# The update notice reads GitHub's latest release, and tags are plain versions

At startup, every version of the tool asks GitHub's API for this repo's latest release (`/releases/latest`, which skips drafts and pre-releases) and reads its tag as a version exactly as written, such as `1.1.0`. Nothing is stripped first, so a tag such as `v1.1.0` isn't a version and shows no notice.

This is the hardest decision in the tool to change, because the versions already out there keep doing it. Renaming the repo, moving releases off GitHub, or tagging differently leaves every older version without a notice, or with a wrong one, and organizers on those versions are the ones who most need it. `docs/releasing.md` has the steps that keep the tag right.

## Considered Options

- **Stripping a leading `v`.** Rejected: it guards against a slip the written release steps already prevent.
- **A JSON library or Octokit** to read the response. Rejected: one field doesn't need one (see ADR 0002).
