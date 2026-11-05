# Window look: a mascot, an icon and a tidier window

Status: ready-for-agent

Blocked by: global-hotkey (its hotkey label gets its final place here)

## Problem Statement

The tool works, but it looks like a 1990s dialog box:

- **Font:** WinForms' old default, Microsoft Sans Serif 8.25pt.
- **Width:** at 300 px, the title bar cuts "Mocchi Macro" short.
- **Icon:** the generic WinForms one in the title bar and taskbar, and a blank .exe on the Desktop.

For a community tool going public at 1.0.0, the first impression matters: organizers decide in seconds whether a downloaded .exe looks trustworthy and cared for. There's also nothing on screen that says "Monster Rancher".

## Solution

A pixel-art Mocchi, the **mascot** (see `GLOSSARY.md`), sits on the left of the window and reacts to what the tool is doing:

- **Idle:** it stands still and blinks every few seconds.
- **While Go types:** it curls into a ball and back out, over and over, like the Mocchi on MR2's loading screen.
- **Done:** it hops.
- **A problem:** it droops with a sweat drop.

The window gets a modern stock Windows look. Segoe UI 9pt, even margins, a bigger Go button, the controls to the right of the mascot, and a width of about 420 px, so the title shows in full. The look stays stock: no custom colours or custom-drawn controls. The tool gets a Mocchi-face icon in its title bar and taskbar, and on the .exe itself.

The art is original, drawn for this tool in Mocchi's likeness, not taken from the game. It's made release-quality, not as a placeholder, and the organizer (the repo owner) approves it before any window code uses it.

## User Stories

1. As an organizer, I want the tool to look tidy and modern, so that I trust it enough to run it and don't mind it being on screen.
2. As an organizer, I want to see "Mocchi Macro" in full in its title bar, so that I can find it among my windows.
3. As an organizer, I want a Mocchi icon on the .exe, the title bar and the taskbar, so that I can spot the tool at a glance.
4. As an organizer, I want the mascot to animate while Go types, so that I can see at a glance that typing is under way.
5. As an organizer, I want the mascot to hop when typing is done and droop when something went wrong, so that I get the outcome before reading the status line.
6. As an organizer, I want the mascot to idle and blink between battles, so that the window feels alive without being distracting.
7. As an organizer at Windows display scaling above 100%, I want the mascot crisp and the controls uncut, so that the tool looks right on my setup.
8. As the repo owner, I want to approve the art before it's built into the window, so that what ships is art I'm happy to release.
9. As a future contributor, I want the art kept as editable source with a way to regenerate it, so that it can be adjusted or redrawn later.

## Implementation Decisions

- **Original art, not game sprites.** Mocchi belongs to Koei Tecmo. Taking sprites from the game files would put their art in a public MIT repo, the most likely way to get a takedown. Original art in Mocchi's likeness is what fan tools commonly do; the release spec's README adds a not-affiliated line.
  - Rejected: **sprites taken from the game files.**
  - Rejected: **no Mocchi** (a plain keyboard icon). It loses the character that makes the tool feel like part of the community.
- **Release quality, from the start.** The art is drawn to ship, not as a placeholder. Mocchi's shape and colours are matched from official art, looked up as reference while drawing. The animations borrow the loading screen's motion, a curl into a ball and back; a motion idea isn't anyone's art.
- **Approval comes first.** The art is the first ticket, and the repo owner's approval is its acceptance criterion. The window tickets wait for it. Previews for approval:
  - an enlarged sheet of every frame
  - a local HTML page that plays each animation at its real timing
  - the icon at its real sizes.
- **Sprite size.** 32×32 pixels per frame, with a transparent background.
  - It's shown at a fixed 3× (96 px) at every display scaling. A 32-pixel sprite only stays crisp at whole multiples, and the repo owner was happy with 3× once they'd seen it at 150%. The controls and the window still grow with scaling, so the mascot looks smaller beside them the higher scaling goes.
  - It's drawn without smoothing (nearest neighbour), and the mascot's area in the window is sized from the frame.
  - Rejected: **a scale that grows with display scaling** (4× at 125% and 150%, 6× at 200%). It was the first plan. It's one more rule to keep, and 3× looked fine.
- **The animations.**

  | Animation | When | Plays | Frames (rough) |
  |---|---|---|---|
  | Idle | Not typing | A still pose, with a blink every few seconds | 1 + 1–2 for the blink |
  | Typing | While Go types | Loops: curls into a ball and back out | 4–6 |
  | Done | Go's outcome is Done | Once: a hop | 3–4 |
  | Problem | Any Go that ends in a problem, including an empty box | Once: droops, sweat drop | 2–3 |

  - After Done or Problem, the last frame holds for about 2 seconds, then idle resumes. A new Go cuts it short.
  - The typing loop runs at its own steady speed, not one frame per key click. Tying it to clicks would mean the typist reporting every key to the window, for a difference nobody sees at 10 keys/s.
  - Startup messages, such as a taken hotkey, don't trigger Problem: that animation is about Go.
- **How the art is made and kept.**
  - **The source of truth** is the frames written as text pixel grids, one character per palette colour, in an `art/` folder at the repo root.
  - **A small generator,** `art/make-art.cs`, turns them into the PNG frames and the `.ico`. It's compiled with the `csc.exe` that ships with .NET Framework (`C:\Windows\Microsoft.NET\Framework64\v4.0.30319\`) and uses `System.Drawing`, so nothing new is installed. That compiler only knows C# 5: no `$"..."` strings, no `=>` members, no `?.`. Its header says how to build and run it.
  - **The generated files,** the PNGs and the `.ico`, are committed too, so a future artist can redraw them in a pixel editor and skip the grids.
  - **In the .exe,** the PNGs and the icon are embedded as resources, so it stays a single file.
  - Rejected: **Python with Pillow.** It's a new tool to install.
  - Rejected: **drawing the grids at runtime.** It leaves no PNGs for an artist to edit, and the `.ico` has to exist at build time anyway.
- **The icon.** It shows Mocchi's face, not the whole body, so it reads at small sizes.
  - It holds 16, 32, 48 and 256 px. The 16 and 32 are hand-placed grids; 256 is the 32 enlarged 8×, and 48 is the 32 at its own size, centred, since 48 isn't a whole multiple of 32.
  - One `.ico` serves as the .exe's icon (the project's application icon) and the main window's icon, so the title bar and taskbar match the Desktop.
- **The layout.**
  - **Font:** Segoe UI 9pt for the whole window. It's set in the window's constructor, after `InitializeComponent`, and not in the designer. Set in the designer, it arrives while WinForms still has scaling to do, so the fixed-size labels scale twice above 100%, and the oversized status line covers the game dropdown. So the designer's view shows the default font.
  - **Scaling:** WinForms scales the layout by the exact DPI (`AutoScaleMode.Dpi`). The usual `AutoScaleMode.Font` left the controls too small for Segoe UI's text above 100%.
  - **Arrangement:** the mascot on the left, the controls in a column to its right, even margins. The Go button is bigger. The hotkey label from the global-hotkey spec gets its final place here.
  - **Width:** about 420 px of client area, enough for "Mocchi Macro 1.0.0" (the release spec puts the version in the title) to show in full in the title bar.
  - **What stays:** the window is still fixed-size, with no maximize (single-exe spec), and every control keeps its behaviour.
  - Rejected: **a custom MR2-coloured theme** with custom-drawn buttons. A lot more WinForms code, and custom drawing tends to break at high scaling, the trouble that sank single-exe ticket 03.
- **No ADR.** Every choice here is cheap to change later.

## Testing Decisions

- The typist and the keyboard layouts are unchanged, so their automated tests keep passing. The window is checked by hand, as before.
- **The art** is judged by the repo owner from the previews (ticket 01).
- **The layout:** at 100%, 125% and 150% scaling:
  - nothing is cut off or overlapping
  - the title shows in full
  - the mascot is crisp, not blurred.
- **The icon:** it shows in the title bar, the taskbar and Alt+Tab, and on the .exe in Explorer and on the Desktop at small, medium and large icon sizes.
- **The animations:**
  - typing loops for as long as Go types
  - Done hops once, then idles after about 2 s
  - a game-not-found Go and an empty-box Go both droop once
  - a Go during the hold cuts it short
  - idle blinks every few seconds.
- **Everything else** in the window works as before: Go, Enter, the hotkey, the typing rate, the game dropdown and the tooltip.

## Out of Scope

- A custom theme, dark mode, or custom-drawn controls.
- Hand-drawn 20, 24 and 40 px icon sizes. At 125% and 150% scaling, Windows shrinks the 32 px icon for the title bar, which softens it slightly; add those sizes later if it looks poor.
- The README screenshot, the version in the title, and the update notice (release-1.0.0 spec).

## Further Notes

- Decided in a grilling session on 2026-11-01, along with the global-hotkey and release-1.0.0 specs. `GLOSSARY.md` gained **Mascot** then: "Mocchi" alone stays a monster name an organizer types.
- The loading screen reference is the repo owner's description: Mocchi curling into a ball and back out, over and over. The owner isn't attached to that exact motion, only to it looking good.
- 2026-11-05, from ticket 02 at real 150% scaling:
  - The mascot's scale changed from growing with display scaling to a fixed 3×. The growing scale was built on `Form.DeviceDpi`, which reports 96 at every scaling unless the app's config file turns on .NET's DPI handling, and the tool ships without one. So the mascot stayed at 3× by accident, and the owner chose to keep that size.
  - Check layout changes at real display scaling. A simulation with a larger font and a faked DPI passed at 125% and 150%, and hid both this and the double-scaled labels.
