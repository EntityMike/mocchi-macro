# 05: About box

Status: ready-for-agent

Blocked by: 01, 02

Parent: [spec](../spec.md)

## What to build

A small "?" button in the top-left corner of the tool's window, above the mascot, opens the About box. Its tooltip says "About".

**The button:**
- It sits within the existing margin above the mascot. It doesn't overlap the mascot or change the window's size.
- It comes last in the tab order, so Tab from the search text still reaches Go first.
- Like the other inputs, it's disabled while Go types. Opening a window then would take the game out of the front partway through a name.

**The About box** is a small fixed-size window titled "About Mocchi Macro", centered on the tool's window. The idle mascot's still pose stands on the left, at the same fixed 3× as in the main window. On the right, in this order:

1. **Mocchi Macro 1.0.0**: the name and version, as in the title bar.
2. What the tool does, in one line.
3. **© 2026 EntityMike · MIT license**
4. Three links, each opening in the default browser:
   - **Help and changelog:** the repo's page on GitHub, `https://github.com/EntityMike/mocchi-macro`, which shows the README and its link to the changelog.
   - **Support:** the Monster Rancher Discord, `https://discord.gg/dfdvXxFHBz`.
   - **Get Monster Rancher 1 & 2 DX on Steam:** `https://store.steampowered.com/app/1716120/Monster_Rancher_1__2_DX/`
5. **Thanks:** Koei Tecmo, for releasing Monster Rancher 1 & 2 DX, and the Monster Rancher community.
6. The README's not-affiliated line: not affiliated with or endorsed by Koei Tecmo, and Monster Rancher and Mocchi are theirs.
7. **Enjoy!**

An **OK** button closes it, and so do Enter and Esc.

**Rules it follows:**
- **One source for the details.** The name, the one-line description and the copyright are read from the .exe's file details (ticket 01), as the title bar reads the version. They're never written out a second time.
- **No network use.** Nothing is fetched. A link only opens the browser when clicked. If there's no default browser, nothing happens, as with the update notice.
- **The hotkey isn't blocked.** Pressing it while the About box is open closes the box, then brings up the search text as usual. A forgotten About box mustn't stop the next search text.
- **The window's rules apply to it too:** DPI-aware scaling, and the font set after the designer's layout, so its text and links aren't cut off at higher display scaling.

**Glossary:** the term **About box** is in `GLOSSARY.md`. Use it in code and comments.

**Changelog:** add the About box to `CHANGELOG.md`'s 1.0.0 entry.

## Acceptance criteria

- [ ] The "?" button sits in the top-left corner, doesn't overlap the mascot, and the window is the same size as before. Check this at real 100%, 125% and 150% scaling.
- [ ] Hovering over the button shows "About". It's last in the tab order, and disabled while Go types.
- [ ] The About box shows the mascot and all seven parts in order, with nothing cut off, at 100%, 125% and 150%.
- [ ] Its name, version, description and copyright match the .exe's Properties → Details.
- [ ] Each of the three links opens the right page in the browser.
- [ ] OK, Enter and Esc each close it.
- [ ] With the About box open, the hotkey closes it and leaves the search text selected in the tool's window.
- [ ] `CHANGELOG.md`'s 1.0.0 entry mentions the About box.
- [ ] The typist and keyboard layout tests still pass, and so does the live check (run by the repo owner).

## User stories covered

11
