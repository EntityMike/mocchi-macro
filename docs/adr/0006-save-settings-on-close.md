# Settings are saved once, on close

The game and the typing rate are saved when the tool's window closes, not when they change. Each save rewrites `user.config`, and a quick run of arrow clicks on the typing rate box rewrote it so fast that anti-ransomware software took the tool for ransomware and suspended it.

## Consequences

- A change made in a run that ends without the window closing, such as a crash or ending the tool from Task Manager, isn't saved.
- A new setting is saved by the same one save on close. Don't save it on change.
