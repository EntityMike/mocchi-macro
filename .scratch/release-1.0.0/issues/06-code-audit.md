# 06: Audit the code before release

Status: ready-for-agent

Blocked by: 05 (the last ticket that changes code)

Parent: [spec](../spec.md)

## What to build

Review the code that will ship as 1.0.0 for glaring issues, so they're fixed before the first public release. Read the whole codebase, not just a recent diff: 1.0.0 is the first time organizers other than the repo owner will run it.

**What to look for**, most important first:

1. **Crashes.** Anything that could crash the tool, or bring up WinForms' crash dialog mid-tournament. That includes exceptions on background threads and in async event handlers.
2. **Wrong clicks.** Anything that could make Go click the wrong place or the wrong window, or keep clicking after it should stop.
3. **Other organizers' PCs.** Things that work on the repo owner's PC but could go wrong elsewhere:
   - another display scaling, or several monitors
   - a keyboard layout other than US, or Windows in another language
   - no default browser, or no network
   - a settings folder that can't be written to
4. **Long sessions.** Leaks or slowdowns over hours of use, since a tournament runs that long: images, handles, timers.
5. **Win32 calls.** Mistakes in signatures or in handling their errors.
6. **Stale docs.** Comments, file headers, `CLAUDE.md`, or the README that no longer match the code.
7. **Dead code, and tests that don't test what their names say.**

**Screen size and key positions.** The repo owner has wondered for a while whether the way key positions follow the game's size is generic enough. Answer that question as part of the audit.

Each game's key positions are measured in a 1600×900 picture. Before each click, the typist finds the game's picture: the largest 16:9 area that fits the client area, centered. It then scales the key's position into that picture. Check whether this holds up, and whether anything depends on 1600×900 more than it should. In particular:

- Is 1600×900 only a unit of measure, with nothing else assuming that size?
- Does the game really draw its keyboard scaled evenly with the picture, at every size it offers, windowed and fullscreen? Say which sizes were checked against the real game and which are only assumed.
- What happens when the client area isn't 16:9 for some other reason? For example, a window resized by hand, a game option that stretches the picture, borderless windowed, or a portrait monitor.
- Is the rounding right at the smallest window size? Do positions come out right on a monitor to the left of or above the main one, where screen positions are negative?
- Does it hold up under display scaling, and across monitors with different scaling?

End with a recommendation, ranked like any other finding. Either it's generic enough as it is, and why, or say what should change and what that would fix.

Review the main project first, then the tests, the live check, and the art generator. The project constraints in `CLAUDE.md` count as standards, so breaking one is a finding. They include the single .exe, C# 7.3, no NuGet packages in the main project, and the typist never sleeping or reading the time itself.

**Not in scope:** new features, refactors for taste, and style changes that make no difference. Every finding has to name a way something goes wrong.

**How it ends:**

- **Findings are recorded.** They go in this ticket's comments, ranked in three groups: must fix before release, worth fixing, and fine to leave. Each finding says:
  - where it is
  - what goes wrong, and for whom
  - how likely that is
  - a suggested fix
- **The repo owner decides on each one.** Small fixes are made under this ticket, with a test wherever the code has a seam for one. Bigger ones become their own tickets, numbered before publishing so it stays the last ticket, or are noted for after 1.0.0. Fixes to unreleased code don't go in the changelog, since 1.0.0 is the first release.

## Acceptance criteria

- [ ] Every file in the main project has been read. The tests, the live check, and the art generator have been checked against the list above.
- [ ] The findings are in this ticket's comments, ranked, and each names what goes wrong.
- [ ] The screen size question is answered with a recommendation: generic enough as it is, or what to change.
- [ ] The repo owner has decided on each finding: fix now, give it its own ticket, or leave it.
- [ ] Every "fix now" finding is fixed, and the full test suite passes.
- [ ] The live check passes after the fixes (the repo owner runs it).

## User stories covered

12

## Comments

**2026-11-22, the audit.** Every file in the main project was read, then the tests, the live check, and the art generator, against the ticket's list, plus `CLAUDE.md`, the README, the changelog, `docs/releasing.md`, and ADR 0008. The build is clean and all 104 tests pass before any fix. Findings are numbered once across the three groups, most important first within each.

### Must fix before release

**1. Go keeps clicking after the game leaves the front.**
- **Where:** `Typist.Type`'s click loop (`Typist.cs`, lines 93–110). The typist confirms the game is in front once, before the first BS, and after that only checks that its window still exists.
- **What goes wrong:** if anything else takes the front while Go types, every remaining click lands on whatever window is at that key's spot: the organizer pressing Alt+Tab or the Windows key, a program popping up a focused window, or the organizer clicking somewhere between clicks. On a stream PC that could be OBS or a chat window. If the game is minimized partway (Win+D, Win+M), its client area reads as 0×0 at -32000,-32000, and the clicks land wherever Windows clamps the cursor, at the edge of the screen. At 3 keys/s or faster, the twelve BS clicks on one spot are also double-clicks, so in a file list they'd open whatever is under them.
- **For whom, how likely:** any organizer, during the 2.5 s a 12-character name takes at 10 keys/s, or up to 24 s at 1 key/s. Unlikely on any one Go, but plausible over the hundreds of Go runs in a tournament, and what the clicks can hit has no limit.
- **Suggested fix:** check `IsInFront()` before every click, beside the `GameStillExists()` check, and stop with a new outcome whose status line says the game left the front partway through, so typing stopped. Test it with a `FakeGamePort` that leaves the front after a few clicks: no clicks follow. As a second half, which no automated test can reach, `User32GamePort.Click` can check again after its 50 ms settle, right before the press, and skip the press if the game is gone or no longer in front. That also covers the game closing during the settle, which today can put one press on the window behind it. A side benefit: Alt+Tab becomes a way to stop a Go that's typing the wrong name, which there's none of today.

### Worth fixing

**2. A damaged settings file stops the tool from starting, every time.**
- **Where:** the first read of `Settings.Default`, in `MainWindow`'s constructor (`MainWindow.cs`, lines 61–68), before `Application.Run`.
- **What goes wrong:** if `user.config` is damaged, .NET throws `ConfigurationErrorsException` ("Configuration system failed to initialize"). Nothing catches it, so the process dies before its window shows, on every start, until someone deletes the file from `%LOCALAPPDATA%\MocchiMacro\MocchiMacro.exe_Url_<hash>\1.0.0.0\`. An organizer won't know to.
- **How likely:** low. The usual cause is a power cut or a hard reset while the file is being written, and the tool writes it on close, which includes Windows shutting down with the tool open. When it does happen, the tool can't be used at all.
- **Suggested fix:** catch `ConfigurationErrorsException` around the first settings read, delete the file it names, and call `Settings.Default.Reload()`, so the tool starts on the defaults. This needs a reference to `System.Configuration.dll`, which is part of .NET Framework, so the .exe stays one file and no package is added. There's no seam for an automated test; check it by hand by writing garbage into `user.config`.

**3. A settings folder that can't be written to brings up WinForms' crash dialog on close.**
- **Where:** `MainWindow_FormClosed` (`MainWindow.cs`, line 381).
- **What goes wrong:** `Settings.Default.Save()` throws when it can't write the file, for example when anti-ransomware software blocks the write (which this project has met once already), on a locked-down profile, or on a full disk. The exception shows as WinForms' "Unhandled exception" dialog, with a stack trace, as the tool closes. The settings are lost either way.
- **How likely:** low, and it happens at close, not mid-tournament. The ticket names this case.
- **Suggested fix:** a `try`/`catch` around the save that does nothing, as the link handlers do, with a comment saying the typing rate and game just aren't kept.

**4. The game window is found by its title alone.**
- **Where:** `User32GamePort.FindGame` (`User32GamePort.cs`, line 40): `FindWindow(null, "MF")` or `"MF2"` matches any top-level window with that exact title, ignoring case, and returns the frontmost one.
- **What goes wrong:** another window titled `MF` or `MF2` gets typed into instead of the game. On Windows 10, File Explorer's window title is just the folder's name, so an Explorer window open on a folder named `MF2` qualifies. If it's above the game, or the game isn't running, Go brings it to the front and clicks into it, the twelve BS clicks open whatever file is under them (finding 1), and the status line says Done instead of "not found".
- **How likely:** low. It needs a window with exactly that title, though a folder named after the game's short name, for saves or backups, isn't far-fetched.
- **Suggested fix:** also match the game window's class, `FindWindow(className, title)`, or check the found window's process name. Either needs the real class or .exe name, read once from each running game (for example with Spy++ or `Get-Process`). No automated test can reach it, since it's in the real port.

**5. Two stand-ins are dead keys on many keyboard layouts.**
- **Where:** `KeyboardLayout.AllStandIns`, and the README's stand-in table.
- **What goes wrong:** on German, French, Spanish, Portuguese, and other layouts, `^` and `` ` `` are dead keys: pressing one shows nothing, and the next letter comes out accented. So `^` then `a` gives `â`, which the box drops with a note, and the name loses a letter. Pressing Space after the dead key gives the stand-in itself, and pasting the symbol works, but nothing tells the organizer so.
- **For whom, how likely:** organizers outside the US and UK who type △ (MR2 DX) or ·. Fairly likely for them, since the Monster Rancher community is international. It costs a retry, not a wrong click.
- **Suggested fix:** a README line under the stand-in table: on keyboards where `^` or `` ` `` is a dead key, press Space after it. No code change.

**6. Stale comments and headers,** each of which misleads someone reading the code cold:
- `MocchiMacro.Tests.csproj`'s header says the tests "drive the typist through a fake game port". Most test classes now cover other things: the art, the footer band, the update check, the tool details, and the changelog.
- `MainWindow.cs`'s header says the hotkey's label is "at the bottom". The footer row is below it now. The README's "under the typing rate" is right.
- `User32GamePort.cs`'s header says the click and timing follow the keyboard test proven against the game. The click has changed since: its 50 ms settle came from global-hotkey ticket 01.
- `Program.cs` has no file header, only the template's comment.
- **Suggested fix:** one-line edits.

### Fine to leave

**7. Screen size and key positions: generic enough as it is.** The full answer is in the next comment. No change before release.

**8. A game running as administrator gets no clicks, and Go still says Done.** Windows silently drops `SendInput` from a normal program into an elevated one (UIPI), and neither the return value nor `GetLastError` says so. It only happens if the organizer runs Steam or the game as administrator, which nothing asks them to. A README line can cover it if anyone reports it.

**9. No last-resort handler for unexpected exceptions** (`Program.cs`). The audit found nothing that can throw on the UI thread, or in Go's or the update check's background work, other than findings 2 and 3: the typist's input is checked first, the Win32 calls don't throw, and the browser and network calls are caught. If something unforeseen did throw, WinForms' dialog offers Continue, which keeps the tool running. A catch-all would hide bugs rather than fix them.

**10. Closing the tool mid-Go can leave the left mouse button held down.** The typing runs on a background thread, which ends with the process. If that happens between a press and its release, Windows treats the button as down until the organizer clicks. It needs the tool closed during the seconds it types, while the cursor keeps being moved away from it. Rare, and one click clears it.

**11. The tool's own window doesn't rescale on a monitor with different scaling.** Already decided in single-exe ticket 03 (wontfix). It doesn't affect where clicks land: those are in real pixels on every monitor.

**12. No leaks over long sessions.** Mascot frames are enlarged once and kept, the footer band is built once, there's one timer and one `HttpClient`, and each Go's game port holds no handles. The only things not disposed are the About box's two `Font` objects per opening, which their finalizers free, so nothing builds up.

**13. The art generator deletes the old PNGs before it has checked every grid** (`make-art.cs`, lines 53–57). A grid with a mistake leaves the frames after it missing. The main project still builds, since it embeds `art/png/*.png` by wildcard, and the tool would crash the first time a missing frame shows, such as the first droop. But the generator prints the error, and while the automated tests load only `idle-1` and the band's tiles, the live check would catch the rest. Fine to leave, or check every grid before writing anything.

**14. Dead code.** `KeyboardLayout.Enter`, both games' Ent. positions, is never used, since the typist never clicks Ent., and no test or live check has ever checked those positions. `Properties/Resources.resx` and `Resources.Designer.cs` are the empty template. `Program.cs` has unused `using`s. Nothing goes wrong today. The one risk is a later "click Ent." feature trusting `Enter`'s unchecked numbers.

**Checked and found fine:** the Win32 signatures (`INPUT` is the right size on 32- and 64-bit, `BOOL` returns marshal correctly, `FindWindow` binds to its Unicode version, and `ShowWindow`'s return isn't misread as success); no network (caught, silent) and no default browser (caught in both link handlers); Windows in another language (nothing reads localized text, and versions and settings parse the same in every culture); the hotkey under other layouts (it registers virtual keys, which every Latin layout has); and the project constraints: C# 7.3, one .exe with no NuGet packages, and a typist that never sleeps or reads the time itself. No test was found that doesn't test what its name says.

**2026-11-22, the screen size question.**

*Is 1600×900 only a unit of measure?* Yes. In the shipping code it appears only as `KeyboardLayout.ReferenceWidth` and `ReferenceHeight`, which `Typist.ToScreen` divides by. Nothing assumes the game runs at that size. The tests' `FakeGamePort` and the live check's fake game window default to it only because the screenshots are that size.

*Does the game draw its keyboard scaled evenly with the picture at every size?* The game's launcher (`docs/screenshots/*/resolutionsettings.png`) offers seven windowed sizes, from 854×480 to 1920×1080, all 16:9 or within a pixel of it, plus Full Screen. It has no option to stretch the picture.
- **Checked against the real game:** MR2 DX windowed at 854×480, 1600×900, and 1920×1080 at 100% scaling, and at 1600×900 at 125% or 150%; MR2 DX fullscreen at 3440×1441 (21:9, so with bars at the sides), and fullscreen again at 125% or 150%. MR1 DX windowed at 1600×900, also at 125% or 150%, at one other windowed size, and fullscreen once (mr1-support ticket 02 doesn't say which size or monitor).
- **Only assumed:** windowed 1024×576, 1136×640, 1280×720, and 1366×768 in either game; fullscreen on a 16:9 monitor; and fullscreen on a monitor taller than 16:9 (16:10 or 4:3), where the bars would be above and below the picture, which only the unit test at 1920×1200 covers. Since the smallest and largest windowed sizes and a fullscreen with bars all landed, the sizes in between are very likely fine. Fullscreen taller than 16:9 is the one real gap: if the game stretched its picture there instead of adding bars, keys would land too high or too low.

*A client area that isn't 16:9 for some other reason:*
- **A window resized by hand, or forced borderless by a third-party tool:** the typist assumes bars around a 16:9 picture. If the game stretched its picture to fill instead, clicks would be off in proportion to how far the window is from 16:9. Whether the game's window can be resized at all is unknown; nothing in the launcher suggests it.
- **A game option that stretches the picture:** there isn't one.
- **A portrait monitor in fullscreen:** treated like any monitor taller than 16:9, with the picture as a band across the middle. Assumed, like 16:10.

*Rounding at the smallest size, and negative screen positions:* bars and key positions are both cut to whole pixels, so a click is off by under 1 px. At 854×480 the keys are about 40 px apart, so that's far inside a key. Each key's position is worked out as an offset into the picture, which is never negative, and only then added to the client area's corner. So cutting off the fraction always rounds down, even when the corner is negative on a monitor to the left of or above the main one. That's correct, but no test pins it; one test with the client area at, say, -1920,-200 would.

*Display scaling, and monitors with different scaling:* the tool is PerMonitorV2-aware, so `GetClientRect`, `ClientToScreen`, and `SetCursorPos` all work in real pixels on every monitor, whatever each monitor's scaling and however the game itself handles scaling. Checked against the game at 125% and 150% on one monitor. A game on a second monitor whose scaling differs from the main one hasn't been checked, but nothing in the path depends on which monitor the game is on.

**Recommendation (fine to leave): generic enough as it is.** The reference size is only a unit, finding the picture covers every size the launcher offers and fullscreen on any monitor where the game adds bars, and the arithmetic holds at the smallest size and at negative positions. Nothing needs to change before release. Two cheap additions, if wanted: the negative-position test above, and a line in ADR 0008 listing the sizes checked against the real game. If an organizer ever reports misses in fullscreen on a 16:10 or 4:3 monitor, that's the first place to look.

**2026-11-23, the repo owner's decisions, and the fixes.**
- **1, fixed.** Before every click, and once after the last, the typist stops with a new outcome, `GameLeftFront`, if another window has taken the front; its status line reads "The game lost the front partway through, so typing stopped." The real port skips a press if the game has closed or left the front during its 50 ms settle. Two typist tests cover leaving the front partway through and during the last click. The README now says Alt+Tab stops typing.
- **2, fixed, and not the way first suggested.** Catching the error at the first settings read and calling `Reload()` didn't work: checked by hand, the tool deleted the file and then stopped anyway, because .NET's settings system keeps failing for the rest of the run once it has failed to read the file. Instead, `Program.Main` now opens `user.config` on its own first, before anything reads `Settings.Default`, and deletes it if it's damaged. Only a file named `user.config` is ever deleted, and a failed delete is ignored, so the check itself can never stop the tool. Checked by hand on a copy of the .exe: a valid file was kept with its saved game, while a file cut off halfway, one of zero bytes, one with a broken tag, and one with a typing rate that isn't a number were each deleted at start, the tool opened on the defaults, and a valid file was saved on close. A settings file that can't be read at all, rather than damaged, still stops the tool at start, as before; that needs someone to lock their own settings folder, so it's left.
- **3, fixed.** The save on close ignores a failure. Checked by hand with the settings folder denied new files and the file read-only: the build before the fix hung on WinForms' crash dialog at close, and the build after it closed at once.
- **4, deferred** until after 1.0.0, with no ticket yet.
- **5, fixed in the README only,** with no code change: a line under Gotchas says to press Space after `^` or `` ` `` on layouts where they combine with the next key.
- **6, fixed.** The four stale headers are corrected, and `AssemblyInfo.cs` gained a short header in place of its comment below the `using`s.
- **7–12, left,** as recommended. The screen size recommendation is accepted, with no change.
- **13, fixed.** `make-art.cs` makes every file in memory first, then clears the old PNGs and writes. Checked: regenerating changes no file, and a broken grid in a copy of `art/` stops the run with all 18 PNGs untouched.
- **14, partly fixed.** `KeyboardLayout.Enter` stays, since a later feature may use it. The empty resources template (`Properties/Resources.resx` and `Resources.Designer.cs`), the unused `using`s in `Program.cs` and `AssemblyInfo.cs`, and the main project's unused framework references (`System.Data`, `System.Data.DataSetExtensions`, `System.Deployment`, `System.Xml`, `System.Xml.Linq`, and `Microsoft.CSharp`) are removed. So that `Enter`'s positions are no longer unchecked, the live check now draws a blue ring at Ent. (END in MR1 DX) on every picture it saves. Drawn on the two keyboard screenshots, the ring sits on each key's label.

All 107 tests pass. The live check is still to be run, by the repo owner; its pictures should show the blue ring on Ent. and END.
