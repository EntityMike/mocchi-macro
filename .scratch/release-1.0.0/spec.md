# Release 1.0.0: publish the tool on GitHub

Status: ready-for-agent

Blocked by: global-hotkey, window-look

## Problem Statement

The tool is ready for organizers, but they have no way to get it:

- The repo is private, with no releases, and the README is a single heading.
- The .exe's file details say "MocchiMacro" with no description or author, which is what Task Manager shows.
- Nothing tells an organizer which version they're running.
- Nothing tells them when a newer one is out. Whoever downloads the first version never hears of the next unless the first version checks.

## Solution

Version 1.0.0 ships as one `MocchiMacro.exe` on the repo's GitHub Releases page, and the repo goes public. Before that, the tool gets:

- **The version in its title bar:** "Mocchi Macro 1.0.0".
- **Proper file details:** product name, description, copyright, and version.
- **An update notice:** at startup, the tool asks GitHub for the latest release. If it's newer, a link in a new footer row reads "Version 1.1.0 is out: download" and opens the releases page. Otherwise the row shows a still band of pixel-art grass.
- **An About box:** a small "?" button in the window's top-left corner opens a window saying what the tool is and who made it, with links to help, support, and the game.

The README is rewritten for organizers. The repo owner builds and publishes the release from written steps, kept for every later release too, and release notes. What changed in each release goes in a new `CHANGELOG.md`, which the README links to. Before publishing, the whole codebase is audited for glaring issues, and the fixes the repo owner picks are made.

## User Stories

1. As an organizer, I want to download the tool as one .exe from a GitHub release, so that getting it is one click.
2. As an organizer, I want a README that says what the tool does and how to use it, so that I can decide whether it's for me and get going quickly.
3. As an organizer, I want the README to warn me about SmartScreen and antivirus, and tell me what to do, so that I'm not scared off by "Windows protected your PC".
4. As an organizer, I want to know which hotkeys the tool uses and what they take from other programs, so that I'm not surprised when another program's shortcut stops working while the tool runs.
5. As an organizer, I want to see the version in the title bar, so that I can say which one I'm on when asking for help.
6. As an organizer, I want the tool to tell me quietly when a newer version is out, with a link to it, so that I don't miss fixes.
7. As an organizer who is offline, or when GitHub doesn't answer, I want the update check to fail silently, so that it never gets in my way.
8. As an organizer, I want the .exe's details to name the tool and its author, so that Task Manager and the file's Properties show something recognizable.
9. As the repo owner, I want written steps for building and publishing the release, so that I can do it, and every later release, without guessing.
10. As an organizer, I want the window to keep its size and look finished whether or not a newer version is out, so that nothing jumps or looks empty after startup.
11. As an organizer, I want an About box in the tool, saying who made it and where to get help, so that I know where to turn without hunting for the README.
12. As the repo owner, I want the whole codebase audited before the first public release, so that glaring problems are fixed before organizers run into them.

## Implementation Decisions

- **Version in the title bar.** The title is built at startup from the assembly version, as three parts: "Mocchi Macro 1.0.0". The window-look spec sized the window for it.
- **File details.** In the assembly info:

  | Attribute | Value | Shows as |
  |---|---|---|
  | `AssemblyTitle` | `Mocchi Macro` | File description in Properties → Details, and Task Manager |
  | `AssemblyProduct` | `Mocchi Macro` | Product name |
  | `AssemblyDescription` | One line: types monster names into MR1/MR2 DX's keyboard | Comments, not shown in Explorer on Windows 10/11 |
  | `AssemblyCompany` | Left blank | Not shown in Explorer's Details, but it names the settings folder (see below) |
  | `AssemblyCopyright` | `© 2026 EntityMike` | Copyright |
  | `AssemblyVersion`, `AssemblyFileVersion` | `1.0.0.0` | File and product version |

  `AssemblyTitle` is the one that matters most, since Windows shows it as the file's description. SmartScreen doesn't use these: for an unsigned file, it shows the file's name and "Unknown publisher".
- **`AssemblyCompany` stays blank.** .NET Framework names the settings folder after the company, or after the program when there's none. Blank keeps `user.config` in `%LOCALAPPDATA%\MocchiMacro\`, a folder an organizer can recognize. The author's name still shows, in the copyright, the field Explorer displays.
  - Rejected: **`EntityMike` as the company.** It shows nowhere an organizer looks, and it moves the settings to `%LOCALAPPDATA%\EntityMike\`.
  - Setting a company in a later version would move the settings too, so organizers' saved values would go back to their defaults.
- **The update notice.**
  - **When:** once, at startup, off the UI thread so the window opens at once. It's a single `HttpClient` GET to `https://api.github.com/repos/EntityMike/mocchi-macro/releases/latest`, with a `User-Agent` header (GitHub requires one) and a short timeout (about 10 s).
    - `/latest` skips drafts and pre-releases.
    - .NET Framework 4.8 uses the operating system's TLS settings, so GitHub's TLS 1.2 works on Windows 10/11 with no extra code. That depends on the .exe stating its 4.8 target, which a build from the project does. A bare `csc.exe` build states none and fails with "Could not create SSL/TLS secure channel", so a test of this has to state it.
  - **Reading the version:** `tag_name` is read from the response with a regular expression (`"tag_name"\s*:\s*"([^"]+)"`) and parsed as a `Version`. Nothing is stripped first, so a tag such as `v1.1.0` doesn't parse and shows no notice. Tags are plain versions, and `docs/releasing.md` says so.
    - Rejected: **a JSON library.** One field doesn't need one, and the framework's own serializers need data classes and a new reference for no gain.
    - Rejected: **stripping a leading `v`.** It guards against a slip the written release steps already prevent.
    - Rejected: **Octokit.** It could be packed into the .exe (Costura.Fody, ILRepack, or a hand-written `AssemblyResolve` handler loading it from a resource), but that's a ~730 KB package and a build step or loader, against an 80 KB tool, for one GET. The earlier tool the update notice is adapted from ships it inside its .exe only because it's .NET 6, published self-contained as one 138 MB file. Its `GetAll` also fetches every release and takes the first, which can be a pre-release.
  - **Comparing:** both versions are cut to three parts (major, minor, build) before comparing. `Version` counts a missing fourth part as lower than `.0`, so "1.0.0" vs "1.0.0.0" would otherwise compare as older. The decision is a plain function of the tag and the running version, so it's unit-tested without the network.
  - **The footer row:** the window grows one row taller, about 18 px at 100%, across its full width under the mascot and the controls. The row is always there, so the window's size never changes when the notice appears.
    - **No newer release:** the row shows the footer band, a still strip of grass with a few small flowers. It's a small set of tiles, such as plain grass, a tuft, and two kinds of flower, laid side by side in a fixed, hand-picked order. That way it looks hand-placed rather than stamped, without drawing one long strip.
      - The tiles are pixel grids in the mascot's palette, kept in `art/` and generated by `make-art.cs` like the mascot frames, and embedded the same way. The order is a line in the same grid file, copied into the code with a test that fails if the two differ, as the mascot's `play` timings are.
      - Every tile shares the same left and right edge columns, so any tile can sit beside any other.
      - The order is long enough for the window at 200% scaling and starts over if the row is wider. The row's image is built once at startup, at the mascot's fixed 3×, so the band fills the row at any display scaling.
      - It's still, because the mascot already moves while Go types.
    - **Its place:** above 100% the row grows with the window (about 27 px tall at 150%) while the band stays 18 px, so the band sits on the row's bottom edge, keeping the grass at the very bottom of the window.
    - **A newer release:** the band gives way to a link label centered in the row, both ways: "Version 1.1.0 is out: download". The whole text is the link, not just "download", so it's a bigger target and nothing in it reads as plain text. Clicking it opens `https://github.com/EntityMike/mocchi-macro/releases/latest` in the default browser, and it stays after a click. No blinking and no message box, so nothing distracts mid-tournament.
    - Rejected: **an empty row when there's no update** (this spec's first plan). Most of the time it would look like a layout mistake.
    - Rejected: **the link beside the hotkey label.** The label takes up to 145 px and the link 163 px, in a 288 px column. Shorter text ("1.1.0 is out: download") fits with about 21 px to spare, but the repo owner chose the full wording.
    - Rejected: **one band grid as wide as the window.** The window grows with display scaling while the art stays at 3×, so a band drawn 420 px wide would fall short at 125% and above.
    - Rejected: **one strip drawn for the widest window and cropped,** or cut into pieces laid out in order, which comes to the same thing. It's the most art to draw, and at 100% only the first half of a strip drawn for 200% would ever show.
    - Rejected: **one tile repeated across the row.** It's the least code (a tiled background image), but the repeat shows every 60 px.
  - **Any failure** (offline, a timeout, rate limiting, a 404 while the repo is private, a tag that isn't a version) is caught, and nothing is shown: the band stays. Unauthenticated, GitHub allows 60 requests an hour per address, plenty for one check per start.
  - Rejected: **a setting to turn the check off.** It's one request per start, and the README says what it sends where.
  - Rejected: **the blinking warning icon and message box** from that earlier tool. They get noticed, but they're distracting on stream, and the message box's URL can't be clicked.
- **Settings across updates aren't needed yet.** The fix, calling `Settings.Default.Upgrade()` on a new version's first run, is code that ships in that new version and reads the old one's saved settings. So a later version can still add it and carry over 1.0.0's settings, if it's ever wanted. There's no ticket for it: the one there was got dropped on 2026-11-20.
- **The README,** for organizers, in this order:
  1. What the tool does, with an animated screenshot of it typing.
  2. A download link to the latest release, and a link to the changelog.
  3. Requirements: Windows 10 (version 1903+) or Windows 11, and MR1 DX or MR2 DX. .NET Framework 4.8 is already part of Windows.
  4. How to use it, hotkey loop first, then what an organizer needs around it:
     - Have the game on its keyboard screen, and pick the game in the dropdown.
     - Leave the mouse alone while Go types: the tool moves the real cursor and clicks.
     - The search text holds 12 characters. The 12th moves the game on by itself, so there's no Ent. to press. Ent. is labeled `END` in MR1 DX.
     - Characters the keyboard has no key for are dropped, with a note saying so. Stand-ins type its symbols, such as `@` for ○, and the box's tooltip lists them.
     - The status line and the mascot both say how typing went.
     - In fullscreen, the tool shows over the game for a few seconds, so viewers on a stream may see it.
  5. The three hotkeys and their fallback order. While the tool runs, the hotkey doesn't reach other programs, even the one in front.
  6. Typing rate advice: 10 keys/s is tested; slow it down if keys get missed.
  7. "Windows protected your PC": the .exe isn't signed, so choose More info → Run anyway.
  8. Antivirus: the tool sends clicks like a person would, which some antivirus finds suspicious, and what to do if it's blocked. Same goes for anti-ransomware software.
  9. The update notice: at startup, the tool asks GitHub once for the latest release, which is the only network use. A link appears when a newer one is out, and nothing happens if GitHub can't be reached.
  10. Where settings are saved (`%LOCALAPPDATA%\MocchiMacro\`), and that a new version, or moving the .exe, starts them over from the defaults.
  11. Not affiliated with or endorsed by Koei Tecmo; Monster Rancher and Mocchi are theirs. The mascot art is original and under the repo's MIT license. The game screenshots in `docs/screenshots/` are theirs too, and the license doesn't cover them.
  12. One line on building from source: open `mocchi-macro.slnx` in Visual Studio and build Release.
  - The animated screenshot is recorded by the repo owner with a screen recorder of their choice. The ticket doesn't add a recording tool. It's deferred and doesn't block the release: until it's recorded, the README keeps a hidden marked spot, and adding it later needs no release.
- **Building and publishing,** by the repo owner.
  - **`docs/releasing.md`** holds the steps every release repeats: bump `AssemblyVersion` and `AssemblyFileVersion` and add the version's entry to `CHANGELOG.md`, build Release, create the release on GitHub with a tag that matches, and attach the .exe. CLAUDE.md gets one line pointing to it and saying the tag must match `AssemblyVersion`. It's written with the update notice (ticket 03), since that's what makes the tag rule matter. The publish ticket adds 1.0.0's one-time steps and links to it for the rest.
    - Rejected: **the steps in the README,** which is for organizers, and **in CLAUDE.md**, by the repo owner's choice; it gets the pointer instead.
  - **Build:** a Release build on their machine.
  - **Tag:** `1.0.0`, a plain version with no `v`, matching `AssemblyVersion`. A `v` tag would show no update notice.
  - **Asset:** one, the bare `MocchiMacro.exe`. No zip and no `.pdb`.
  - **Where:** the release is created on GitHub's website, with release notes written in the ticket. They're short, a line saying what the tool is and a link to the README, which has the SmartScreen note and links to `CHANGELOG.md`. They don't list the changes.
  - **Before going public:** Go types into both real games, which needs the VS mode server up. Typing into a real game in the new window is still unseen (window-look tickets 02 and 04), and the first public version shouldn't be its first real test.
  - **Visibility:** the repo goes public as it is (history, `.scratch/`, `CLAUDE.md`, game screenshots), just before the release is published. The live check's fake game window needs those screenshots, and the window-look spec names game art in a public repo as the likeliest way to get a takedown. It's accepted here, with the README saying whose they are.
  - Rejected: **a GitHub Actions build.** Worth it if releases get frequent, not for the first one.
  - Rejected: **installing the `gh` CLI.** It's a new tool, for a step done once per release.
  - Rejected: **code signing.** It costs about $10 a month (Azure Artifact Signing, formerly Trusted Signing); the README explains SmartScreen instead.
- **The About box,** opened by a small "?" button in the window's top-left corner. Ticket 05 has the full contents.
  - **A small window, not a message box.** It shows the trophy frame (new art of Mocchi sitting in a giant gold trophy cup, one still frame), the text, clickable links, and an OK button.
    - Rejected: **a message box.** It's one line of code, but its URLs can't be clicked, the same reason the update notice doesn't use one.
  - **What it says:** the name and version, what the tool does, the copyright and license, links to the repo's page (for the README and changelog), the Monster Rancher Discord, and the game on Steam, thanks to Koei Tecmo and the Monster Rancher community, the README's not-affiliated line, and "Enjoy!". The name, description, and copyright are read from the .exe's file details, so they're written once.
    - The support link is the Discord, since organizers are already there and the repo doesn't use GitHub issues.
  - **No network use.** The links only open the browser when clicked, so the update check is still the tool's only network use.
- **No ADR.** Release mechanics are recorded here and in `docs/releasing.md`.

## Testing Decisions

- The typist and the keyboard layouts are unchanged by the new features, so their automated tests keep passing. A fix from the audit that touches them comes with its own test.
- **The live check** (`live-check` spec) passes before publishing.
- **Go in the real games:** typing works in both, in the new window, and window-look's unseen Done messages read correctly: `END` in MR1 DX, `Ent.` in MR2 DX, and the 12-character message.
- **Title and details:** the title reads "Mocchi Macro 1.0.0". The Release .exe's Properties → Details show the description, product, copyright, and version, and Task Manager shows "Mocchi Macro".
- **Settings:** they're still saved under `%LOCALAPPDATA%\MocchiMacro\`, so the file details don't reset anything.
- **The update notice:**
  - **The decision,** in automated tests: a newer, equal, older, and unreadable tag, a `v` tag, and `1.1.0` against `1.1.0.0`.
  - **No release yet:** while the repo is private or has no release, the tool starts normally with the band and no delay, and online and offline behave the same.
  - **A newer release, before 1.0.0 exists:** with the repo the check asks about pointed for one run at another public repo with a release (latest tag `1.3.0`), the tool shows "Version 1.3.0 is out: download" in place of the band.
  - **A newer release, for real:** once 1.0.0 is published, a build with its version temporarily set to `0.9.0` shows "Version 1.0.0 is out: download", and the link opens the releases page.
  - **Up to date:** the real 1.0.0 shows the band, no link.
- **The footer band:** the repo owner approves its art before the window uses it, as with the mascot. An automated test fails if its order in the grid file and in the code differ.
- **Layout:** with the footer row and the "?" button added, nothing is cut off or overlapping at real 100%, 125%, and 150% scaling, with the band and with the link, and the band fills the row's width.
- **The About box,** by hand at real 100%, 125%, and 150%: nothing cut off, its details match the .exe's file details, each link opens the right page, and the hotkey closes it.
- **The audit's fixes** are tested as the audit ticket says, and the live check passes after them.
- **The README** renders correctly on GitHub, and its download link reaches the release.
- **The published release** has the tag `1.0.0` and one asset, `MocchiMacro.exe`. Downloaded fresh on another machine or user account, it runs after SmartScreen's Run anyway.

## Out of Scope

- Keeping settings across updates and .exe moves.
- Self-updating, an installer, and code signing.
- A GitHub Actions build.
- Presets, a hotkey that types, and clicking Ent.

## Further Notes

- Decided in a grilling session on 2026-11-01, along with the global-hotkey and window-look specs.
- The update notice is adapted from an earlier tool's, without Octokit or the blinking icon.
- 2026-11-05, reviewed against the code after global-hotkey, window-look, and live-check landed:
  - The update link moved to its own row, since the row beside the hotkey label had no room for it.
  - `AssemblyCompany` was found to move the settings folder, checked by asking .NET Framework for the path with and without it, so it stays blank.
  - The README list gained what the how-to was missing, and the update notice.
  - SmartScreen's dialog was corrected from web sources, not tested on a real download. Ticket 07's download step will show it first-hand.
  - Going public now waits for Go to be seen working in both real games, and for the live check to pass.
- 2026-11-20, the update notice was grilled again against the earlier tool's code and release:
  - The empty row became the footer row, with the band when there's no update.
  - The leading `v` is no longer stripped, and the release steps moved to `docs/releasing.md`.
  - Octokit's real cost was recorded: it can be packed into the .exe, but isn't worth it.
  - `GLOSSARY.md` gained **Release** and **Update notice**.
  - The tickets were renumbered in build order: the band became 02, the notice 03, the README 04, and publishing 05.
- 2026-11-21, while writing the README, the repo owner chose to keep the changes out of the GitHub release: `CHANGELOG.md` was added, newest release first, and the release notes link to it.
- 2026-11-21, after the README was written, the repo owner added the About box as ticket 05, so publishing became 06.
- 2026-11-21, the repo owner then added a whole-codebase audit as ticket 06, after the About box, so publishing became 07.
- 2026-11-23, the repo owner cut the release notes down to a line about the tool and a link to the README, rather than repeating the README's SmartScreen note and linking to the changelog.
