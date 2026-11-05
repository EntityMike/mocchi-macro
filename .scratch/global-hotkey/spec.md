# Global hotkey: bring the tool to the front from anywhere

Status: resolved

## Problem Statement

To type a search text, the organizer has to reach the tool's window with the mouse: find it on another monitor, or dig it out from behind the game, then click the search text box and clear what's in it. Doing that between every pair of battles is slow. A global hotkey was always planned, but as a second way to trigger typing. Without a preset list, a hotkey that types has nothing new to type, so what saves time now is getting to the box quickly.

## Solution

A **hotkey** (see `GLOSSARY.md`) brings the tool's window to the front from any program, restoring it if it's minimized, with the search text box focused and its text selected. The organizer types the next search text over it and presses Enter (or Go). Typing brings the game to the front, as it does today, so when typing ends the game is in front again, ready for Ent. The loop for each search text:

hotkey → type the search text → Enter → check the monster field and press Ent. in the game → repeat

The hotkey is fixed, not changeable. The tool tries three key combinations in order and keeps the first one no other program has: **Ctrl+Alt+Shift+M**, then **Ctrl+Shift+F9**, then **Ctrl+Alt+Shift+F9**. A label in the window always shows which hotkey is in use. If the tool had to use a fallback, or got none, the status line says so once at startup. Go works the same with or without a hotkey.

## User Stories

1. As an organizer, I want one key combination to bring the tool's window to the front from any program, so that I don't have to find it with the mouse between battles.
2. As an organizer, I want the search text box focused with its text selected when the hotkey brings the window up, so that I can type the next search text straight over the last one.
3. As an organizer, I want the hotkey to restore the tool's window when it's minimized, so that I can keep it out of the way between battles.
4. As an organizer, I want Enter to type the search text and leave the game in front, so that I can press Ent. in the game straight away.
5. As an organizer playing fullscreen, I want the same loop to work, accepting that the tool shows over the game while it's in front, so that fullscreen play needs no other steps.
6. As an organizer, I want the hotkey to do nothing while the tool is typing, so that typing isn't broken by the tool taking focus mid-name.
7. As an organizer, I want the tool to fall back to another combination when another program already has Ctrl+Alt+Shift+M, so that I still get a hotkey.
8. As an organizer, I want to see which hotkey the tool is using at any time, so that I know what to press even on a day it had to fall back.
9. As an organizer whose tool fell back to another combination, or got none, I want to be told once at startup, so that I'm not left pressing a combination that does nothing.
10. As an organizer whose tool got no hotkey, I want Go to keep working, so that the tool is still usable.

## Implementation Decisions

- **The hotkey brings the window up; it doesn't type.** On the hotkey, the main window is restored if minimized, brought to the front and activated. Then the search text box gets focus with all its text selected. If the window is already in front, the hotkey does the same, so pressing it always leaves the box ready.
  - Windows normally refuses to let a background program take focus. A program that receives a hotkey message is allowed to, since the organizer's key press went to it. If that turns out not to hold, record it in the ticket and look for a workaround before going further.
- **Ignored while typing.** While Go types (the time its inputs are disabled), the hotkey does nothing. Activating the tool then would take focus from the game partway through a name.
- **Fullscreen.** A fullscreen game doesn't minimize when it loses focus. It stays behind whatever window is in front, so the hotkey puts the tool over it, and Go brings the game back to the front as it does today. This is accepted: viewers may see the tool over the game for a few seconds. Avoiding it needs typing without leaving the game, which waits for the preset list.
- **The three combinations, in order:**

  | Order | Combination | Why |
  |---|---|---|
  | 1 | Ctrl+Alt+Shift+M | The default. M for Mocchi; almost nothing claims three modifiers. |
  | 2 | Ctrl+Shift+F9 | A function key, rarely taken globally, and outside the Ctrl+Shift+letters other programs claim. |
  | 3 | Ctrl+Alt+Shift+F9 | The same function key with three modifiers, for when even that is taken. |

  - Avoided: **Ctrl+Shift+M, and Ctrl+Shift+letter in general.** It was the first default, but programs that run in the background, such as graphics driver software, often claim Ctrl+Shift+letters for their own hotkeys out of the box, Ctrl+Shift+M among them. On those PCs the tool would fall back every time. Found on 2026-11-02, when it was taken on the dev PC.
  - Ctrl+Alt+Shift+M's catch: Ctrl+Alt acts as AltGr, so on the few layouts where AltGr+Shift+M types a character, the tool takes that character while it runs. Accepted, since it's rare and the fallbacks cover a combination another program has.
  - Avoided: **Ctrl+Alt+M.** On German and many other European keyboards, Ctrl+Alt acts as AltGr, and AltGr+M types characters such as µ. Taking it would break their typing in every program.
  - Avoided: **F12 in any form.** Windows reserves it for debuggers.
  - Every combination has a modifier. A bare key such as F9 is taken from every other program while the tool runs.
- **Registered once, at startup.** The tool tries the combinations in order when its window is created and keeps the first that registers. It doesn't retry later if another program lets one go. Combinations are registered with no auto-repeat, so holding them down doesn't fire again and again. The hotkey is released when the window closes.
  - Rejected: **a changeable hotkey** (a box the organizer presses a combination into). The fallback list covers a taken default without the extra UI. It can be added later if organizers ask for it.
  - Rejected: **a low-level keyboard hook.** It watches every key press like a keylogger does, and antivirus treats it that way.
- **What the window shows.**
  - **The hotkey label**, always visible, for example `Hotkey: Ctrl+Alt+Shift+M`, or `Hotkey: none` when nothing registered. Its final place comes with the window-look spec. Here it goes somewhere sensible in today's window, which may grow taller to fit it.
  - **On a fallback**, the status line shows, once at startup and in the problem red: "Ctrl+Alt+Shift+M is taken by another program, so the hotkey is Ctrl+Shift+F9." (or Ctrl+Alt+Shift+F9).
  - **With none**, it shows: "All three hotkeys are taken by other programs, so there's no hotkey. Typing still works."
  - **With the default**, nothing is said beyond the label.
  - Rejected: **a popup at startup.** It interrupts stream setup and leaves nothing behind once closed. The label answers "what's my hotkey?" at any time.
- **The cost of a global hotkey.** While the tool runs, its combination no longer reaches other programs, even the one in front. Three modifiers make that unlikely to matter for Ctrl+Alt+Shift+M; a fallback such as Ctrl+Shift+F9 is likelier to be some program's shortcut. The release spec's README says so.
- **No ADR.** Changing what the hotkey does later is cheap, and the reasons are recorded here and in `GLOSSARY.md`.

## Testing Decisions

- Nothing here touches the typist or the keyboard layouts, so their automated tests keep passing unchanged. The hotkey and the window are checked by hand, like the rest of the main window.
- **The loop:** with MR2 DX windowed and the tool behind it or on another monitor, the hotkey brings the tool up with the old text selected. Typing a new search text and pressing Enter types it into the monster field, and the game is in front afterwards. Repeat a few times.
- **Fullscreen:** the same loop with MR2 DX fullscreen. The game stays behind the tool on the hotkey and comes back to the front on Enter.
- **Minimized:** with the tool minimized, the hotkey restores it.
- **While typing:** pressing the hotkey partway through typing changes nothing, and the name is typed correctly.
- **Fallbacks:** each running copy of the tool takes one combination. So start four copies:
  - The first gets Ctrl+Alt+Shift+M and says nothing.
  - The second gets Ctrl+Shift+F9, with its status line message.
  - The third gets Ctrl+Alt+Shift+F9, with its message.
  - The fourth gets none, with its message, and its Go still works.

  Close the first copy and start a new one: it gets Ctrl+Alt+Shift+M again.

## Out of Scope

- A hotkey that types, presets, and clicking Ent.
- Changing the hotkey, or retrying a taken combination later.
- Keeping the tool from showing over a fullscreen game while it's in front.
- Stopping a second copy of the tool from running.
- The window's final layout (window-look spec) and the README (release-1.0.0 spec).

## Further Notes

- Decided in a grilling session on 2026-11-01, along with the window-look and release-1.0.0 specs. `GLOSSARY.md` gained **Hotkey** then.
- 2026-11-02: the order changed from Ctrl+Shift+M, Ctrl+Alt+Shift+M, Ctrl+Shift+F9 to today's, after another program's default hotkeys were found holding Ctrl+Shift+M (see the combinations table).
- 2026-11-02: testing showed a fullscreen game doesn't minimize when it loses focus, as first assumed; it stays behind the window in front. The fullscreen wording was corrected to match.
- The hotkey was first planned as a second way to trigger typing, like Go. That meaning was replaced here.
- 2026-11-05: the none message used to end "Use the Go button.", which read as if Go replaced the hotkey. Go is needed with or without one, so the wording was corrected.
