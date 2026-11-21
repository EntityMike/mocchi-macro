# Releasing

The steps every release repeats, done by the repo owner. A release's one-time steps, if it has any, live in its own ticket.

1. **Bump the version.** In `MocchiMacro/Properties/AssemblyInfo.cs`, set both `AssemblyVersion` and `AssemblyFileVersion` to the new version, such as `1.1.0.0`.
   In `CHANGELOG.md`, add an entry for the new version, such as `## 1.1.0`, listing what changed. The changes live there, not in the GitHub release's description.
2. **Build Release.** From the repo root in Git Bash:

   ```bash
   dotnet build mocchi-macro.slnx -c Release
   ```

   Or build Release in Visual Studio. The .exe is `MocchiMacro/bin/Release/MocchiMacro.exe`, and its title bar should read "Mocchi Macro" and the new version.
3. **Create the release on GitHub** (the repo's Releases page → Draft a new release), with a new tag that matches the version as a plain number, such as `1.1.0`, with no `v`. Leave it unmarked as a pre-release.
4. **Attach `MocchiMacro.exe` only.** No zip and no `.pdb`: the tool is the one file.

## Why the tag must match the version

The update notice compares the latest release's tag with the running tool's `AssemblyVersion`, cut to three parts. The tag is read as a version exactly as written, so:

- A tag such as `v1.1.0` isn't a version, and organizers on older versions never see the notice.
- A tag that doesn't match the version bumped in step 1 makes the notice wrong. A tag ahead of it makes the new release itself show a notice for a version that doesn't exist. A tag behind it means older versions never hear of it.
