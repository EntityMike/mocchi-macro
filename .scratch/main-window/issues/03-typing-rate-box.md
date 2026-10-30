# 03: Typing rate box

Status: ready-for-agent

Blocked by: 01

Parent: [spec](../spec.md)

## What to build

Let the organizer set the typing rate instead of the fixed 10 keys per second.

- A number box with arrows (`NumericUpDown`) at the bottom of the window. It takes whole numbers 1–10, default 10.
- Go passes its value to the typist as the typing rate.
- The typing rate is saved in the project's existing user settings when it changes, and restored at startup.

## Acceptance criteria

- [ ] The box accepts only whole numbers 1–10, and changes with the arrows.
- [ ] On first run it shows 10.
- [ ] After changing it and restarting the tool, it shows the changed value.
- [ ] Test: at typing rates 1 and 4, click *n* happens at *n* × 1000/rate ms on the fake clock, BS clicks included.
- [ ] By hand: at typing rate 2, "Mocchi" visibly types about two keys per second.

## User stories covered

11, 12, 13, 14, 15
