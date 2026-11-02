# 03: Update notice

Status: ready-for-agent

Blocked by: 02

Parent: [spec](../spec.md)

## What to build

**The check:** at startup, off the UI thread, send one `HttpClient` GET to `https://api.github.com/repos/EntityMike/mocchi-macro/releases/latest`, with a `User-Agent` header and a timeout of about 10 s.

- Read `tag_name` with a regular expression (`"tag_name"\s*:\s*"([^"]+)"`) and parse it as a `Version`, without stripping anything: a `v` tag doesn't parse and shows nothing.
- Cut both it and the assembly version to three parts before comparing.
- Keep the decision a plain function of the tag and the running version, with unit tests: a newer, equal, older and unreadable tag, a `v` tag, and `1.1.0` against `1.1.0.0`.

**When the release is newer:** the footer band (ticket 02) gives way to a link label centred in its row, both ways: "Version X.Y.Z is out: download". The whole text is the link, not just "download". It opens `https://github.com/EntityMike/mocchi-macro/releases/latest` in the default browser and stays after a click. The window's size doesn't change.

**Any failure** is caught and leaves the band in place, without slowing startup. No new references or packages are added: `System.Net.Http` is already referenced.

**The release steps:** write `docs/releasing.md` with the steps every release repeats:

1. Bump `AssemblyVersion` and `AssemblyFileVersion` to the new version.
2. Build Release.
3. Create the release on GitHub with a tag that matches the version as a plain number, such as `1.1.0`, with no `v`.
4. Attach `MocchiMacro.exe` only.

Say why the tag must match: the update notice compares the two, and a `v` or a mismatch means organizers never see it. Add one line to CLAUDE.md pointing to the file and saying the tag must match `AssemblyVersion`.

## Acceptance criteria

- [ ] The new unit tests pass, along with the existing ones.
- [ ] While the repo is private (GitHub answers 404), the tool starts at once with the band and no link, online and offline.
- [ ] With the network cable out or Wi-Fi off, the tool starts at once with the band, no link and no error.
- [ ] With the repo the check asks about pointed for one run at another public repo with a release (latest tag `1.3.0`), the tool shows "Version 1.3.0 is out: download" in place of the band. Switch it back afterwards. Record the result in this ticket's comments.
- [ ] With the link showing, nothing is cut off or overlapping at real 100%, 125% and 150% scaling.
- [ ] `docs/releasing.md` exists with the four steps, and CLAUDE.md points to it.

The check against mocchi-macro's own release happens in ticket 05, once 1.0.0 is published.

## User stories covered

6, 7, 9, 10
