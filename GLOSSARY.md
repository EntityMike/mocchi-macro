# Mocchi Macro

A tool that types monster names into Monster Rancher 1 & 2 DX's on-screen keyboard for tournament organizers, so battles get set up faster.

## People

**Organizer**:
The tournament organizer running battles, usually on stream; the person using this tool.
_Avoid_: User, TO, host

## The game

**Game**:
Which Monster Rancher DX game the organizer is playing: MR1 DX or MR2 DX. The organizer picks it in the tool.
_Avoid_: Edition, version

**Game window**:
The running Monster Rancher DX window, titled `MF` (Monster Rancher 1 DX) or `MF2` (Monster Rancher 2 DX).
_Avoid_: Game client, emulator

**Picture**:
The game's 16:9 image inside the game window. It fills a window, but sits centered between black bars in fullscreen on a monitor that isn't 16:9.
_Avoid_: Viewport, canvas

**Keyboard**:
The game's on-screen keyboard on the multiplayer monster search screen, clicked with the mouse. MR1 DX and MR2 DX each have their own, with different keys in different places.
_Avoid_: In-game keyboard, on-screen keyboard (fine in prose, but "keyboard" alone means this one)

**Key**:
One clickable character or command on the keyboard, including **BS** and **Ent.**
_Avoid_: Button (reserved for this tool's own window), letter

**BS**:
The keyboard's backspace key; deletes the last character in the monster field. Labeled `BS` in MR2 DX and `←` in MR1 DX.

**Ent.**:
The keyboard's enter key; runs the search on what's in the monster field. Labeled `Ent.` in MR2 DX and `END` in MR1 DX.

**Monster field**:
The text field at the top of the keyboard screen that the keys type into, labeled `Monster:` in MR2 DX. It holds at most 12 characters, and the 12th moves the game on to the next screen, as Ent. does.
_Avoid_: Search box, name field

## Typing

**Search text**:
The text the tool types into the monster field: a full monster name, or just enough of one to narrow the search.
_Avoid_: Name, input, query

**Stand-in**:
A character the keyboard has no key for, which the search text box swaps for the symbol key it stands for, such as `@` for ○.
_Avoid_: Shortcut, alias

**Typing rate**:
How many keys the tool clicks on the keyboard per second.
_Avoid_: Click rate, speed, rate of button presses

## The tool

**About box**:
The small window the "?" button in the tool's window opens, saying what the tool is, who made it and where to get help.
_Avoid_: Help box, info dialog

**Footer band**:
The strip of pixel-art grass along the bottom of the tool's window, which the update notice takes the place of.
_Avoid_: Footer, banner, grass strip

**Hotkey**:
The key combination that brings the tool's window to the front from any program, with the search text selected so the organizer can type over it. It doesn't type anything itself; Enter or Go does.
_Avoid_: Shortcut, keybind, trigger

**Mascot**:
The pixel-art Mocchi in the tool's window, which animates while Go types. Not the monster Mocchi, which is search text like any other monster name.
_Avoid_: Mocchi (for the art), sprite, logo

**Release**:
A published build of the tool that organizers download, named by its version number, such as `1.1.0`.
_Avoid_: Build, drop

**Update notice**:
The link in the tool's window saying a newer release is out.
_Avoid_: Update alert, version warning
