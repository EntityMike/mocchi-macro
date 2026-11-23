# 05: About box

Status: resolved

Blocked by: 01, 02

Parent: [spec](../spec.md)

## What to build

A small "?" button in the top-left corner of the tool's window, above the mascot, opens the About box. Its tooltip says "About".

**The button:**
- It sits within the existing margin above the mascot. It doesn't overlap the mascot or change the window's size.
- It comes last in the tab order, so Tab from the search text still reaches Go first.
- Like the other inputs, it's disabled while Go types. Opening a window then would take the game out of the front partway through a name.

**The About box** is a small fixed-size window titled "About Mocchi Macro", centered on the tool's window. The trophy frame stands on the left, at the same fixed 3× as in the main window. On the right, in this order:

1. **Mocchi Macro 1.0.0**: the name and version, as in the title bar.
2. What the tool does, in one line.
3. **© 2026 EntityMike · MIT license**
4. Three links, each opening in the default browser:
   - **Help and changelog:** the repo's page on GitHub, `https://github.com/EntityMike/mocchi-macro`, which shows the README and its link to the changelog.
   - **Support:** the Monster Rancher Discord, `https://discord.gg/8bgNyFBvHp`.
   - **Get Monster Rancher 1 & 2 DX on Steam:** `https://store.steampowered.com/app/1716120/Monster_Rancher_1__2_DX/`
5. **Thanks:** Koei Tecmo, for releasing Monster Rancher 1 & 2 DX, and the Monster Rancher community.
6. The README's not-affiliated line: not affiliated with or endorsed by Koei Tecmo, and Monster Rancher and Mocchi are theirs.
7. **Enjoy!**

An **OK** button closes it, and so do Enter and Esc.

**The trophy frame:** new art made for the About box, a nod to the tournaments the tool is for. It's one still frame, not an animation.
- Mocchi sits in a giant gold trophy cup, wider than its body, facing you with its eyes open. Its head, bill, and the top of its body show above the rim.
- Its hands rest on the front of the rim. They're outlined all the way round, so they read as round hands rather than flowing out of the body.
- The cup has a darker band under the rim, two big ear handles from the rim to the lower bowl, a stem, and a wider base, so at 3× it reads as a trophy, not a bowl. A row of pink separates the rim from the bill, which is the same gold.
- Two small gold sparkles beside Mocchi's head say it's shiny.
- It's a 32×32 grid in a new `art/about.txt`, drawn like the other frames and with the same edges, so it lines up with them. It uses only colors already in `palette.txt`. The gold comes from `Y`, `O`, and `A`, with `K` for the cup's outline.
- The file has no `play` line, since there's nothing to play.
- `make-art.cs` adds `about` to its list of grid files, so it writes `art/png/about-1.png`. The main project's existing wildcard embeds it as `Mascot.about-1.png`, with no change to the `.csproj`.
- The About box loads it with `MascotArt.LoadFrame("about-1")` instead of idle's frame 1.
- The repo owner approves the drawing by eye before it's committed.

**Rules it follows:**
- **One source for the details.** The name, the one-line description, and the copyright are read from the .exe's file details (ticket 01), as the title bar reads the version. They're never written out a second time.
- **No network use.** Nothing is fetched. A link only opens the browser when clicked. If there's no default browser, nothing happens, as with the update notice.
- **The hotkey isn't blocked.** Pressing it while the About box is open closes the box, then brings up the search text as usual. A forgotten About box mustn't stop the next search text.
- **The window's rules apply to it too:** DPI-aware scaling, and the font set after the designer's layout, so its text and links aren't cut off at higher display scaling.

**Glossary:** the term **About box** is in `GLOSSARY.md`. Use it in code and comments.

**Changelog:** add the About box to `CHANGELOG.md`'s 1.0.0 entry.

## Acceptance criteria

- [x] The "?" button sits in the top-left corner, doesn't overlap the mascot, and the window is the same size as before. Check this at real 100%, 125%, and 150% scaling.
- [x] Hovering over the button shows "About". It's last in the tab order, and disabled while Go types.
- [x] The About box shows the trophy frame and all seven parts in order, with nothing cut off, at 100%, 125%, and 150%.
- [x] `art/png/about-1.png` comes from `art/about.txt` through `make-art.cs`, and running it again changes no file.
- [x] Its name, version, description, and copyright match the .exe's Properties → Details.
- [x] Each of the three links opens the right page in the browser.
- [x] OK, Enter, and Esc each close it.
- [x] With the About box open, the hotkey closes it and leaves the search text selected in the tool's window.
- [x] `CHANGELOG.md`'s 1.0.0 entry mentions the About box.
- [x] The typist and keyboard layout tests still pass, and so does the live check (run by the repo owner).

## User stories covered

11

## Comments

**2026-11-21:** Built without the trophy frame, showing idle's still pose as the ticket first said (commit 4003320). The repo owner then chose new art for the About box: one still frame with a trophy, now specified above. Drafts of Mocchi holding the cup in front of its body didn't stand out, and the owner chose Mocchi sitting in a giant cup instead. The rest of the About box is unchanged.

**2026-11-22, checked by the repo owner:** The "?" button and the About box look right at real 100%, 125%, and 150% scaling, with nothing cut off. Each link opens the right page, after the Discord invite was swapped for https://discord.gg/8bgNyFBvHp because the first one expired. With the About box open, the hotkey closes it and brings up the search text. The live check passes.

**2026-11-22:** Checked in-process, with the main window built but not shown and the About box opened off-screen, so no real mouse or keys were used. The "?" button's tooltip is "About", it's last in the window's tab order, and it's disabled while Go types and enabled again after. Esc, Enter, and clicking OK each close the About box, with focus starting on OK. Its name, version, description, and copyright match the .exe's file details, which `ToolDetailsTests` checks against the version resource Properties → Details reads. Closed.
