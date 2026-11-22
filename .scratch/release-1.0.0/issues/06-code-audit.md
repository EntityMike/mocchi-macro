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
6. **Stale docs.** Comments, file headers, `CLAUDE.md` or the README that no longer match the code.
7. **Dead code, and tests that don't test what their names say.**

**Screen size and key positions.** The repo owner has wondered for a while whether the way key positions follow the game's size is generic enough. Answer that question as part of the audit.

Each game's key positions are measured in a 1600×900 picture. Before each click, the typist finds the game's picture: the largest 16:9 area that fits the client area, centred. It then scales the key's position into that picture. Check whether this holds up, and whether anything depends on 1600×900 more than it should. In particular:

- Is 1600×900 only a unit of measure, with nothing else assuming that size?
- Does the game really draw its keyboard scaled evenly with the picture, at every size it offers, windowed and fullscreen? Say which sizes were checked against the real game and which are only assumed.
- What happens when the client area isn't 16:9 for some other reason? For example, a window resized by hand, a game option that stretches the picture, borderless windowed, or a portrait monitor.
- Is the rounding right at the smallest window size? Do positions come out right on a monitor to the left of or above the main one, where screen positions are negative?
- Does it hold up under display scaling, and across monitors with different scaling?

End with a recommendation, ranked like any other finding. Either it's generic enough as it is, and why, or say what should change and what that would fix.

Review the main project first, then the tests, the live check and the art generator. The project constraints in `CLAUDE.md` count as standards, so breaking one is a finding. They include the single .exe, C# 7.3, no NuGet packages in the main project, and the typist never sleeping or reading the time itself.

**Not in scope:** new features, refactors for taste, and style changes that make no difference. Every finding has to name a way something goes wrong.

**How it ends:**

- **Findings are recorded.** They go in this ticket's comments, ranked in three groups: must fix before release, worth fixing, and fine to leave. Each finding says:
  - where it is
  - what goes wrong, and for whom
  - how likely that is
  - a suggested fix
- **The repo owner decides on each one.** Small fixes are made under this ticket, with a test wherever the code has a seam for one. Bigger ones become their own tickets, numbered before publishing so it stays the last ticket, or are noted for after 1.0.0. Fixes to unreleased code don't go in the changelog, since 1.0.0 is the first release.

## Acceptance criteria

- [ ] Every file in the main project has been read. The tests, the live check and the art generator have been checked against the list above.
- [ ] The findings are in this ticket's comments, ranked, and each names what goes wrong.
- [ ] The screen size question is answered with a recommendation: generic enough as it is, or what to change.
- [ ] The repo owner has decided on each finding: fix now, give it its own ticket, or leave it.
- [ ] Every "fix now" finding is fixed, and the full test suite passes.
- [ ] The live check passes after the fixes (the repo owner runs it).

## User stories covered

12
