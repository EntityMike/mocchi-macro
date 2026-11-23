# Releasing

The steps every release repeats, done by the repo owner. A release's one-time steps, if it has any, live in its own ticket.

1. **Bump the version.** With every change going into the release already merged to `main`, set both `AssemblyVersion` and `AssemblyFileVersion` in `MocchiMacro/Properties/AssemblyInfo.cs` to the new version, such as `1.1.0.0`.
   In `CHANGELOG.md`, add an entry for the new version at the top, such as `## 1.1.0`, listing what changed. The changes live there, not in the GitHub release's description.
2. **Commit the bump on its own,** straight on `main`, with only those two files: `Release 1.1.0`, with no body, since the changelog entry is the list of changes. It's the release's last commit and the one the tag points at, so the tagged code says the version it's released as, and the release's changes are all in the commits before it.
3. **Build Release and run the tests** from that commit, with nothing uncommitted, so the .exe is exactly the tagged code. From the repo root in Git Bash:

   ```bash
   dotnet build mocchi-macro.slnx -c Release
   MocchiMacro.Tests/bin/Release/net48/MocchiMacro.Tests.exe
   ```

   Or build Release and run the tests in Visual Studio. The .exe is `MocchiMacro/bin/Release/MocchiMacro.exe`, and its title bar should read "Mocchi Macro" and the new version.

   A test fails if the changelog's newest entry isn't the version from step 1, so a bump without its entry, or an entry without its bump, is caught here.
4. **Push `main`,** so GitHub has the bump commit to tag.
5. **Create the release on GitHub** (the repo's Releases page → Draft a new release), with a new tag that matches the version as a plain number, such as `1.1.0`, with no `v`, and `main` as its target. Leave it unmarked as a pre-release.
6. **Attach `MocchiMacro.exe` only.** No zip and no `.pdb`: the tool is the one file.

## Why the tag must match the version

The update notice compares the latest release's tag with the running tool's `AssemblyVersion`, cut to three parts. The tag is read as a version exactly as written, so:

- A tag such as `v1.1.0` isn't a version, and organizers on older versions never see the notice.
- A tag that doesn't match the version bumped in step 1 makes the notice wrong. A tag ahead of it makes the new release itself show a notice for a version that doesn't exist. A tag behind it means older versions never hear of it.
