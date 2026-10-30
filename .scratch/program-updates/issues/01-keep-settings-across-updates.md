# 01: Keep the saved settings across updates and moves

Status: needs-triage

Blocked by: program updates (not specced yet; this folder's `spec.md` will hold it)

## Problem

The typing rate is saved with .NET Framework's default user settings, which keep `user.config` in a folder named after the tool's version and the .exe's path: `%LOCALAPPDATA%\MocchiMacro\MocchiMacro.exe_Url_<hash of the path>\<version>\`. So the organizer loses the saved typing rate, and it's back to 10, when:

- a new version of the tool ships with a higher `AssemblyVersion` (now `1.0.0.0`), or
- the .exe is moved or copied to another folder.

Nothing is lost today, since there's only one version. It matters once updates ship.

## Options

- **Call `Settings.Default.Upgrade()` on the first run of a new version.** The usual pattern is a `SettingsUpgraded` user setting that defaults to `false`. When it's false, call `Upgrade()`, set it to true, and save. It's a few lines, but it only covers version bumps, not moves.
- **Keep the settings in one fixed place,** such as a file in `%APPDATA%\MocchiMacro\` or next to the .exe for a portable tool. This covers both version bumps and moves, but it replaces the built-in settings with our own file or settings provider.

Pick one once it's decided how updates ship (installer, zip, self-update) and whether the .exe is meant to be portable.

## Acceptance criteria

- [ ] After updating to a new version, the tool shows the typing rate set before the update.
- [ ] Decide whether moving the .exe should keep the settings too, and test that if so.

## Comments

- 2026-10-30, found while implementing main-window ticket 03 (typing rate box).
