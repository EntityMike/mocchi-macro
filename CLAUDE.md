# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

A Windows tool that types monster names into Monster Rancher 1 & 2 DX's on-screen keyboard by clicking its keys with the mouse, so tournament organizers set up battles faster. It's a WinForms app on .NET Framework 4.8 that ships as a single `.exe`.

Use the words defined in `GLOSSARY.md` in code, comments, tickets and commit messages.

## Commands

Run from the repo root in Git Bash.

```bash
# Build all three projects
dotnet build mocchi-macro.slnx

# Run every test (build first; the test project is its own executable)
MocchiMacro.Tests/bin/Debug/net48/MocchiMacro.Tests.exe

# Run one test class, or one test
MocchiMacro.Tests/bin/Debug/net48/MocchiMacro.Tests.exe --filter "ClassName~TypistTests"
MocchiMacro.Tests/bin/Debug/net48/MocchiMacro.Tests.exe --filter "Name=TypesTheMiddleDotAtItsKeyOnTheGrid"

# Run the tool
MocchiMacro/bin/Debug/MocchiMacro.exe
```

- `dotnet test` doesn't work here: the tests use MSTest's own runner (`EnableMSTestRunner`), which the .NET 10 SDK's `dotnet test` refuses without extra opt-in. Run the test `.exe` instead.
- There's no linter or formatter configured.

### The live check (don't run it unasked)

`MocchiMacro.LiveCheck/bin/Debug/net48/MocchiMacro.LiveCheck.exe` runs the real tool against a fake game window and checks the mascot's frames and where each click lands. It takes over the mouse for about two minutes and fails if anything else touches the mouse or focus, so only a person starts it. It refuses to start while a real game is running. The header of `MocchiMacro.LiveCheck/Program.cs` says what it can and can't prove.

### Regenerating the art

The header of `art/make-art.cs` has the commands. It's compiled with the C# 5 `csc.exe` that ships with .NET Framework, so it can't use newer syntax.

### Releasing

`docs/releasing.md` has the steps. The release's tag must match `AssemblyVersion` as a plain version such as `1.1.0`, with no `v`, or the update notice never shows.

## Architecture

### Typing: `Typist` behind the `IGamePort` seam

`Typist` holds all the typing logic and reaches the game only through `IGamePort`. `User32GamePort` is the real one (user32.dll calls); `FakeGamePort` in the tests records clicks against a virtual clock, so the typist tests need no game and no real time. Anything that needs the window, mouse or clock goes through the port, including waiting: `Typist` must never sleep or read time itself.

One Go: refuse text the keyboard can't type, find the game window by title, restore it if minimized, bring it to the front and confirm it got there, click BS 12 times to clear the monster field, then click each key. It never clicks Ent. Clicks are scheduled against the clock rather than by fixed gaps, and the client area is re-read before every click so typing follows a moved window.

### Key positions: one reference size

`KeyboardLayout` holds each game's key centers as measured in a 1600×900 picture (the screenshots in `docs/screenshots/`). `Typist` scales them to whatever the game window is now: it first finds the 16:9 picture inside the client area (fullscreen on a non-16:9 monitor has black bars), then scales into it. Layouts never hold screen coordinates.

`Game` ties a game's dropdown name, window title (`MF` / `MF2`), layout and Ent. key label together. Adding a game means a new `Game` and a new layout, nothing else.

Stand-ins (`@` for ○ and so on) are swapped by the search text box in `MainWindow`, not by `Typist`. `Typist` only ever sees characters that have keys, and refuses anything else as a guard.

### The window

`MainWindow` runs `Typist` off the UI thread with a fresh `User32GamePort` per Go, then turns the `TypingOutcome` into the status line and the mascot's reaction. It also owns the search text rules (12 characters, drop characters without a key), the saved settings, and the global hotkey (registered once at startup, with two fallbacks).

Settings are saved once on close, not on each change. Rapid rewrites of `user.config` got the tool suspended by anti-ransomware software.

### The art: text grids to the mascot and the footer band

- `art/*.txt` pixel grids are the source of truth. `art/make-art.cs` turns them into `art/png/*.png` and `art/Mocchi.ico`, which are committed. Never edit the PNGs or the icon by hand.
- The main project embeds those files as resources: the mascot's frames as `Mascot.<frame>.png`, the footer band's tiles as `footer-band-<n>.png`, and `Mocchi.ico`. `MascotArt` loads them and enlarges them 3× without smoothing.
- `MascotAnimation` picks the frame from the time passed in, with no clock or window of its own. Its timings are copied from each grid file's `play` line, and a test fails if the two differ, so change them together.
- `FooterBand` lays the tiles side by side, in the order copied from `art/footer-band.txt`'s `order` line, into one image as wide as the footer row. It's built once at startup. As with `play` lines, a test fails if the two orders differ. `make-art.cs` refuses tiles whose edge columns don't match, since any tile can sit beside any other.

### Project constraints worth knowing

- **`MocchiMacro.csproj` is an old-style project.** A new `.cs` file isn't compiled until it has its own `<Compile Include="..." />` line. The Tests and LiveCheck projects are SDK-style and pick files up by themselves.
- **C# 7.3** throughout (the .NET Framework 4.8 default): tuples are fine; nullable reference types, records, switch expressions and `using` declarations aren't.
- **The tool ships as one `.exe`** with nothing next to it. Art is embedded, DPI awareness is declared in `app.manifest` rather than a config file, and no NuGet packages are referenced by the main project. Keep it that way.
- **DPI.** The tool is PerMonitorV2 aware so click points are real pixels. The window uses `AutoScaleMode.Dpi` and sets its font after `InitializeComponent`; comments in `MainWindow.cs` say what broke otherwise.

## Agent skills

### Issue tracker

Issues live as local markdown files under `.scratch/<feature-slug>/`. See `docs/agents/issue-tracker.md`.

### Triage labels

Default five triage roles, recorded as a `Status:` line in each ticket. See `docs/agents/triage-labels.md`.

### Domain docs

Single-context: one `GLOSSARY.md` + `docs/adr/` at the repo root. See `docs/agents/domain.md`.
