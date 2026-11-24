// The .exe's file details. Windows shows the title as the file's description in
// Properties → Details and as the tool's name in Task Manager. A release bumps both
// versions at the bottom, as docs/releasing.md says.

using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Mocchi Macro")]
[assembly: AssemblyDescription("Types monster names into MR1 DX's and MR2 DX's on-screen keyboards")]
[assembly: AssemblyConfiguration("")]
// Left blank on purpose. .NET Framework names the settings folder after the company, or
// after the program when there's none, so blank keeps user.config in
// %LOCALAPPDATA%\MocchiMacro\. Setting one would move the settings to a folder named
// after the company, and every organizer's saved typing rate and game would go back to
// their defaults.
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Mocchi Macro")]
[assembly: AssemblyCopyright("© 2026 EntityMike")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("89b75152-6a97-4104-9cbd-7b0658109206")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
