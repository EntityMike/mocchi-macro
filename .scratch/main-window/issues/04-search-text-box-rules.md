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
