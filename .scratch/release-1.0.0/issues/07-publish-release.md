# 07: Publish 1.0.0

Status: ready-for-human

Blocked by: 03, 04, 05, 06. Only ticket 04's README text is needed. Its animated screenshot is deferred and doesn't block publishing, and its GitHub rendering and SmartScreen checks happen here.

Parent: [spec](../spec.md)

## What to build

The agent writes the release notes into this ticket's comments: a line saying what the tool is, and a link to the README. The changes live in the changelog, which the README links to, not in the release. The repo owner then publishes, following `docs/releasing.md` (ticket 03) for the steps every release repeats, plus 1.0.0's one-time steps:

1. Merge everything for 1.0.0 to `main`, and check that `AssemblyVersion` and `AssemblyFileVersion` are `1.0.0.0`. Check that `CHANGELOG.md`'s 1.0.0 entry covers everything shipping, including the About box.
2. Build Release, as `docs/releasing.md` says. Check `MocchiMacro/bin/Release/` holds `MocchiMacro.exe` and that its title reads "Mocchi Macro 1.0.0".
3. Run the live check (`MocchiMacro.LiveCheck/Program.cs` says how). It must pass.
4. With that Release .exe, press Go in both real games, which needs the VS mode server up. Check that typing works and that the Done messages read correctly: `END` in MR1 DX, `Ent.` in MR2 DX, and the 12-character message.
5. Make the repo public: GitHub → Settings → General → Danger Zone → Change visibility.
6. Create the release on GitHub as `docs/releasing.md` says, with these for 1.0.0:
   - Tag `1.0.0`, created on `main`, and the title "Mocchi Macro 1.0.0"
   - The release notes from this ticket
   - Not a pre-release.
7. Check the update notice against the live release:
   - A build with `AssemblyVersion` temporarily set to `0.9.0.0` shows "Version 1.0.0 is out: download" in place of the footer band, and clicking it opens the releases page.
   - The real 1.0.0 shows the band and no link.
8. Download the .exe from the release on another machine or user account and run it through SmartScreen.

## Acceptance criteria

- [ ] The live check passes on the commit being released.
- [ ] Before going public, Go typed into both real games, and the Done messages read correctly.
- [ ] The repo is public, and the README renders on GitHub with a working download link.
- [ ] The release `1.0.0` exists, isn't a pre-release, and has exactly one asset, `MocchiMacro.exe`.
- [ ] The downloaded .exe runs after SmartScreen's Run anyway, and its title reads "Mocchi Macro 1.0.0".
- [ ] The update notice showed for the `0.9.0` build and not for the real 1.0.0. Record the result in this ticket's comments.

## User stories covered

1, 9

## Comments

**2026-11-23, the release notes.** Paste everything inside the fence into the release's description. The README link is a full URL because a release's description doesn't resolve paths relative to the repo. The README covers the rest: how to use the tool, the SmartScreen note, and the changelog.

````markdown
Initial release of Mocchi Macro, a tool for Monster Rancher 1/2 DX tournament organizers.

See the [README](https://github.com/EntityMike/mocchi-macro#readme) for more info.

Enjoy!
````
