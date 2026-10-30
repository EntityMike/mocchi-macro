# 04: Search text box rules

Status: ready-for-agent

Blocked by: 01

Parent: [spec](../spec.md)

## What to build

Make the search text box accept only what the keyboard can type, and make it quick to use.

- At most 12 characters.
- Characters without a key are removed as they're typed or pasted, using the keyboard layout's "does this character have a key" check. A note under the box says which characters were dropped, and clears on the next edit that drops nothing.
- Go is the form's accept button, so Enter in the search text box runs it.
- The box starts empty and keeps its text after Go.
- Before clicking anything, the typist checks that every character of the search text has a key. If one doesn't, it reports "search text has a character without a key" and clicks nothing, not even BS. The box already removes such characters; this guards text that reaches the typist another way, such as the preset list later.

## Acceptance criteria

- [ ] Typing a 13th character does nothing.
- [ ] Typing a space or an accented letter leaves the box unchanged and shows a note naming the character.
- [ ] Pasting "Mo cchi é" leaves "Mocchi" in the box and a note naming the space and "é".
- [ ] Pasting `○ × △ □` keeps them.
- [ ] Pressing Enter in the box types the search text, the same as Go.
- [ ] After Go, the search text is still in the box. On a fresh start, the box is empty.
- [ ] Test: a search text with a character that has no key (for example "Mo cchi") reports that outcome, and nothing is clicked.

## User stories covered

3, 4, 5, 6, 7, 8, 16, 17

## Comments

- 2026-10-30, from implementing 01: until this ticket lands, a search text with a character that has no key (a space, "é") crashes Go after the 12 BS clicks have already cleared the monster field. The organizer chose both guards: the box removes such characters, and the typist refuses such text before any click. The status line (ticket 05) shows the new outcome; if this ticket lands first, the outcome is reported but not shown yet.
- 2026-10-30, implemented on branch `main-window`: the typist returns `CharacterWithoutKey` before looking for the game, so nothing is clicked, not even BS. Go still discards the outcome until ticket 05's status line.
- 2026-10-30: checked by hosting the built window in-process and posting it typed characters, Enter, and inserted text in place of a paste (this shell has no clipboard access). It starts empty. A space or "é" leaves the box unchanged with the note "No key, dropped: space" or "...: é", and the next clean edit clears the note. A 13th character is ignored. "Mo cchi é" becomes "Mocchi" with "space, é" noted, and "○ × △ □" become "○×△□". Enter runs Go once, and the text stays after it. A real Ctrl+V paste, and Enter against the game, are still to do by hand.
- 2026-10-30: the 12-character limit is enforced after the characters without a key are dropped, not through the box's `MaxLength`. `MaxLength` cut a paste before dropping, so "Mo cchi é Golem" left "MocchiGo" instead of "MocchiGolem". Characters over 12 are cut from just before the caret, which is the end of what was just typed or pasted. So a typed 13th character is ignored, and a long paste keeps its first characters. A space typed into a full box now gets its note too.
- 2026-10-30: when characters are cut, the note says "Cut to 12", after the dropped characters if there are any ("No key, dropped: space, é. Cut to 12"). This covers story 6 for long pastes, which were cut silently before. The wording is kept short so that both notes together fit the window. The organizer chose to show the note for pastes only, so a typed 13th character that has a key is stopped in `KeyPress` and does nothing at all, with no note. A typed character without a key still gets through, to be dropped and noted.
- 2026-10-30: the note is dark red (`Firebrick`) so it stands out from the window's labels. Ticket 05's status line should use the same red for its failure messages, so red means one thing across the window.
- 2026-10-30, by hand: a real Ctrl+V of "Mo cchi é" left "Mocchi". Go typed into the game. Enter against the game hasn't been tried separately.
- 2026-10-30, by hand (noted while checking 05): Enter in the search text box types into the game, the same as Go.
