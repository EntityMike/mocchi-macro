# Mocchi Macro

Mocchi Macro types monster names into the on-screen keyboard of Monster Rancher 1 DX and Monster Rancher 2 DX for you. Instead of clicking out every letter of a monster's name with the mouse, you type it into the tool and press Enter, and the tool clicks the keys in the game. It's made for tournament organizers, to set up battles faster. Enjoy!

<!-- Animated screenshot of the tool typing goes here, once recorded: ![Mocchi Macro typing a monster name into the game](docs/typing.gif) -->

## Download

Download `MocchiMacro.exe` from the [latest release](https://github.com/EntityMike/mocchi-macro/releases/latest). It's the whole tool: there's nothing to install, so put it anywhere and run it.

What's changed in each release is in the [changelog](CHANGELOG.md).

## Requirements

- Windows 10 (version 1903+) or Windows 11.
- Monster Rancher 1 DX or Monster Rancher 2 DX.

The tool needs .NET Framework 4.8, which is already part of these versions of Windows.

## How to use it

1. Start the game and go to the screen where you search for a monster by name, with the on-screen keyboard showing.
2. Start Mocchi Macro and pick your game, MR1 DX or MR2 DX, in its dropdown. It remembers the game for next time.
3. For each monster:
   1. Press the **hotkey**, Ctrl+Alt+Shift+M, or the one shown in the tool's window if it had to use another (see [The hotkey](#the-hotkey)). The tool comes to the front with the search text selected, so you can type straight over the last one.
   2. Type the monster's name, or just enough of it to narrow the search, and press Enter (or click **Go**).
   3. The tool brings the game to the front, clears the monster field and clicks out the name on the keyboard. Leave the mouse alone while it types: the tool moves your real mouse cursor and clicks with it.
   4. When it's done, the game is in front again. Check the monster field and press Ent. in the game yourself (it's labelled `END` in MR1 DX). The tool never presses it for you.

### Good to know

- **12 characters at most.** The monster field holds 12 characters, so the search text does too. The 12th character moves the game on to the next screen by itself, so after a 12-character name there's no Ent. to press.
- **Characters the keyboard doesn't have are dropped.** A note under the search text says which ones were dropped. Spaces are dropped too, since the keyboard has no space key.
- **Symbols are typed with stand-ins.** To type one of the keyboard's symbols, type its stand-in and the tool swaps it in. Hover over the search text to see the stand-ins for your game.

  | Type | To get | MR1 DX | MR2 DX |
  |---|---|:---:|:---:|
  | `@` | ○ | ✓ | ✓ |
  | `*` | × | ✓ | ✓ |
  | `^` | △ |  | ✓ |
  | `#` | □ |  | ✓ |
  | `` ` `` (backtick) | · | ✓ | ✓ |

  You can also paste the symbols themselves, and the Japanese middle dot (・) is turned into · too.
- **How it went.** After each Go, the status line, between the search text and the typing rate, says how typing went, in green when it's done and red when something went wrong, such as the game not running. The mascot, the little Mocchi in the window, hops when it's done and droops when it isn't.
- **Switching games clears the search text,** so a name meant for one game isn't typed into the other.
- **Fullscreen works,** but the tool shows over the game for the few seconds between the hotkey and Enter. If you stream in fullscreen, viewers may see it.

## The hotkey

The tool tries these key combinations in order and uses the first one no other program has already taken:

1. Ctrl+Alt+Shift+M
2. Ctrl+Shift+F9
3. Ctrl+Alt+Shift+F9

The label under the typing rate always shows which one it's using. If it had to fall back, or all three were taken, it says so when it starts. Go still works without a hotkey.

**While the tool is running, its hotkey doesn't reach any other program, even the one in front.** If another program uses the same key combination for something, it stops working in that program until you close the tool.

## Typing rate

The typing rate is how many keys the tool clicks per second, from 1 to 10. It starts at 10, which is the rate the tool was tested at. If keys get missed, which can happen on a busy PC, turn it down.

## Gotchas

### "Windows protected your PC"

The first time you run the tool, Windows may show a blue box saying "Windows protected your PC". That's because the .exe isn't signed, which costs money every year, not because anything's wrong with it. To run it:

1. Click **More info**.
2. Click **Run anyway**.

### Antivirus

The tool clicks the mouse for you, the way a person would, and some antivirus software finds that suspicious. If yours blocks or deletes the tool, you can add an exception for `MocchiMacro.exe` in its settings.

Same goes for anti-ransomware software.

## The update notice

When the tool starts, it asks GitHub once for the latest release. That's the only time it uses the network, and it only reads the latest release's version number from the answer. If a newer version is out, a link at the bottom of the window says so, such as "Version 1.1.0 is out: download", and opens the download page. If GitHub can't be reached, nothing happens and the tool works as normal.

## Where settings are saved

The tool saves your game and typing rate in `%LOCALAPPDATA%\MocchiMacro\` when you close it. A new release, or moving the .exe to another folder, starts over from the defaults: MR2 DX at 10 keys per second.

## Not affiliated with Koei Tecmo

Mocchi Macro is a fan-made tool, not affiliated with or endorsed by Koei Tecmo. Monster Rancher and Mocchi are theirs, and so are the game screenshots in `docs/screenshots/`.

The tool and its mascot art are original, under this repo's [MIT license](LICENSE). The license doesn't cover the game screenshots.

## Building from source

Open `mocchi-macro.slnx` in Visual Studio and build the Release configuration.
