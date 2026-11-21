# 01: Version in the title bar, and the .exe's file details

Status: resolved

Parent: [spec](../spec.md)

## What to build

- **Title:** build it at startup from the assembly version, as three parts: "Mocchi Macro 1.0.0".
- **Assembly info:** fill in the attributes as the spec's table lists:
  - `AssemblyTitle` and `AssemblyProduct` "Mocchi Macro"
  - a one-line `AssemblyDescription`
  - `AssemblyCopyright` "© 2026 EntityMike"
  - versions `1.0.0.0`.
- **`AssemblyCompany` stays blank:** setting it would move the saved settings, as the spec explains.

## Acceptance criteria

- [x] The window's title reads "Mocchi Macro 1.0.0" and shows in full.
- [x] Changing `AssemblyVersion` to `1.2.3.0` changes the title to "Mocchi Macro 1.2.3" (then change it back).
- [x] The Release .exe's Properties → Details show File description "Mocchi Macro", Product name "Mocchi Macro", Copyright "© 2026 EntityMike", and File and Product version 1.0.0.0.
- [x] Task Manager lists the running tool as "Mocchi Macro".
- [x] The typing rate and game saved before this ticket still come back: settings stay under `%LOCALAPPDATA%\MocchiMacro\`.
- [x] The automated tests pass unchanged.

## User stories covered

5, 8

## Comments

**2026-11-20:** Built. The running tool's title reads "Mocchi Macro 1.0.0", uncut in the title bar at 100% scaling. With `AssemblyVersion` at `1.2.3.0` it read "Mocchi Macro 1.2.3"; changed back. The Release .exe's version resource, which Properties → Details shows, has File description and Product name "Mocchi Macro", Copyright "© 2026 EntityMike", no company, and File and Product version 1.0.0.0. Closing the tool saved its settings into the same `%LOCALAPPDATA%\MocchiMacro\` folder as before. The automated tests pass unchanged.

**2026-11-20, closed by the repo owner:** Task Manager lists the running Release build under Apps as "Mocchi Macro (32 bit)", with its window "Mocchi Macro 1.0.0".
