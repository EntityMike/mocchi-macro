# 03: Update notice

Status: resolved

Blocked by: 02

Parent: [spec](../spec.md)

## What to build

**The check:** at startup, off the UI thread, send one `HttpClient` GET to `https://api.github.com/repos/EntityMike/mocchi-macro/releases/latest`, with a `User-Agent` header and a timeout of about 10 s.

- Read `tag_name` with a regular expression (`"tag_name"\s*:\s*"([^"]+)"`) and parse it as a `Version`, without stripping anything: a `v` tag doesn't parse and shows nothing.
- Cut both it and the assembly version to three parts before comparing.
- Keep the decision a plain function of the tag and the running version, with unit tests: a newer, equal, older, and unreadable tag, a `v` tag, and `1.1.0` against `1.1.0.0`.

**When the release is newer:** the footer band (ticket 02) gives way to a link label centered in its row, both ways: "Version X.Y.Z is out: download". The whole text is the link, not just "download". It opens `https://github.com/EntityMike/mocchi-macro/releases/latest` in the default browser and stays after a click. The window's size doesn't change.

**Any failure** is caught and leaves the band in place, without slowing startup. No new references or packages are added: `System.Net.Http` is already referenced.

**The release steps:** write `docs/releasing.md` with the steps every release repeats:

1. Bump `AssemblyVersion` and `AssemblyFileVersion` to the new version.
2. Build Release.
3. Create the release on GitHub with a tag that matches the version as a plain number, such as `1.1.0`, with no `v`.
4. Attach `MocchiMacro.exe` only.

Say why the tag must match: the update notice compares the two, and a `v` or a mismatch means organizers never see it. Add one line to CLAUDE.md pointing to the file and saying the tag must match `AssemblyVersion`.

## Acceptance criteria

- [x] The new unit tests pass, along with the existing ones.
- [x] While the repo is private (GitHub answers 404), the tool starts at once with the band and no link, online and offline.
- [x] With the network cable out or Wi-Fi off, the tool starts at once with the band, no link, and no error.
- [x] With the repo the check asks about pointed for one run at another public repo with a release (latest tag `1.3.0`), the tool shows "Version 1.3.0 is out: download" in place of the band. Switch it back afterward. Record the result in this ticket's comments.
- [x] With the link showing, nothing is cut off or overlapping at real 100%, 125%, and 150% scaling.
- [x] `docs/releasing.md` exists with the four steps, and CLAUDE.md points to it.

The check against mocchi-macro's own release happens in ticket 07, once 1.0.0 is published.

## User stories covered

6, 7, 9, 10

## Comments

**2026-11-20:** Built. The decision's unit tests pass, along with the existing ones. Clicking the link with no default browser does nothing, rather than show WinForms' crash dialog. Still to check by hand: the private repo online and offline, the one run pointed at another public repo, and the link at real 100%, 125%, and 150% scaling.

**2026-11-20, pointed at another public repo:** With the check's API address set for one run to another public repo with a release, GitHub answered tag `1.3.0`, and the repo owner saw "Version 1.3.0 is out: download" in place of the band, at real 100% scaling, whole and centered, with nothing cut off or overlapping. The address was switched back to mocchi-macro afterward, uncommitted throughout.

**2026-11-21, 125% scaling:** Pointed at that repo again, the repo owner saw the link whole and centered at real 125% scaling, with nothing cut off or overlapping.

**2026-11-21, 150% scaling:** The same at real 150% scaling: the link whole and centered, nothing cut off or overlapping. The address was switched back to mocchi-macro afterward.

**2026-11-21, private repo online:** With the check asking mocchi-macro's own repo, which is private so GitHub answers 404, the tool opened at once with the band and no link.
