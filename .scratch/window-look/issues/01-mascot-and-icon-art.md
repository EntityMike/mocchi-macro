# 01: Mascot and icon art

Status: ready-for-agent

Parent: [spec](../spec.md)

## What to build

Release-quality pixel art of Mocchi, original and drawn in its likeness, plus a way to regenerate it.

- **Reference:** look up official Mocchi art for its shape and colours. Don't copy game sprites.
- **Frames:** 32×32 each, transparent background, written as text pixel grids in `art/`. Animations:
  - **idle:** a still pose plus 1–2 blink frames
  - **typing:** 4–6 frames, a loop that curls into a ball and back out
  - **done:** 3–4 frames, a hop
  - **problem:** 2–3 frames, a droop with a sweat drop.

  Give each animation a frame timing.
- **Icon:** Mocchi's face, as hand-placed 16×16 and 32×32 grids.
- **Generator:** `art/make-art.cs`, compiled with .NET Framework's own `csc.exe` (C# 5 only) and using `System.Drawing`.
  - It writes one PNG per frame, and `Mocchi.ico` with 16, 32, 48 and 256 px. The 48 and 256 are the 32 enlarged by whole steps, without smoothing.
  - A header comment says how to build and run it.
  - Commit the generated files too.
- **Previews** for approval, outside the repo:
  - every frame enlarged on one sheet
  - an HTML page that plays each animation at its timing, at 3× and 4×
  - the icon at its real sizes.

Iterate on the previews until the art looks right, then show them to the repo owner. Their feedback goes in this ticket's comments.

## Acceptance criteria

- [ ] The generator builds with .NET Framework's `csc.exe` and regenerates every PNG and the `.ico` from the grids, byte for byte the same as the committed files.
- [ ] The `.ico` holds 16, 32, 48 and 256 px images, and the face reads clearly at 16.
- [ ] Each animation plays smoothly in the preview, and the typing loop has no visible jump where it repeats.
- [ ] The repo owner has approved the art (noted in this ticket's comments with the date).

## User stories covered

8, 9
