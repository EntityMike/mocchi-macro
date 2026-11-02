# 01: Version in the title bar, and the .exe's file details

Status: ready-for-agent

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

- [ ] The window's title reads "Mocchi Macro 1.0.0" and shows in full.
- [ ] Changing `AssemblyVersion` to `1.2.3.0` changes the title to "Mocchi Macro 1.2.3" (then change it back).
- [ ] The Release .exe's Properties → Details show File description "Mocchi Macro", Product name "Mocchi Macro", Copyright "© 2026 EntityMike", and File and Product version 1.0.0.0.
- [ ] Task Manager lists the running tool as "Mocchi Macro".
- [ ] The typing rate and game saved before this ticket still come back: settings stay under `%LOCALAPPDATA%\MocchiMacro\`.
- [ ] The automated tests pass unchanged.

## User stories covered

5, 8
