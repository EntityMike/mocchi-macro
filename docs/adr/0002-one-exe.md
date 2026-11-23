# The tool ships as one .exe with nothing beside it

Organizers download one file and run it, with nothing to install or unzip and nothing to lose by moving it. So everything the tool needs is inside the .exe: the mascot's frames, the footer band's tiles, and the icon are embedded resources, and DPI awareness is declared in `app.manifest`, so there's no `App.config` and no `MocchiMacro.exe.config` beside the .exe.

The main project references no NuGet packages, since each would be a DLL beside the .exe or a build step to pack it in. The test and live check projects can, because they don't ship.

## Considered Options

- **Octokit for the update notice,** packed into the .exe with Costura.Fody, ILRepack, or a hand-written `AssemblyResolve` loader. Rejected: about 730 KB and a build step or loader, against an 80 KB tool, for one GET that a regular expression can read.
- **DPI awareness in `App.config`.** Rejected: it needs the config file beside the .exe.
