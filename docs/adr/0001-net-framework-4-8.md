# .NET Framework 4.8 and WinForms, not modern .NET

Organizers run the tool on Windows 10 or 11, which already include .NET Framework 4.8, so the tool is a single .exe of about 80 KB with nothing to install. A modern .NET tool would need the organizer to install its runtime, or would ship self-contained as one file well over 100 MB (an earlier tool this one borrows from is 138 MB that way).

## Consequences

- The code is C# 7.3, the .NET Framework 4.8 default: no nullable reference types, records, switch expressions, or `using` declarations.
- The main project is an old-style `.csproj`, so each new `.cs` file needs its own `<Compile Include>` line.
- HTTPS to GitHub relies on the operating system's TLS settings, which works because the .exe states its 4.8 target.
