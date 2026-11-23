// The tool's name, version, description, and copyright, read from the .exe's file details in
// Properties/AssemblyInfo.cs, and where its repo lives. The title bar, the About box, and the
// update check read them here, so each is written once and a release only bumps the version.

using System;
using System.Reflection;

namespace MocchiMacro
{
    public static class ToolDetails
    {
        // The repo on GitHub, as owner/name. Its page and its API address both build on this,
        // so moving or renaming the repo changes one line.
        public const string Repo = "EntityMike/mocchi-macro";
        public const string RepoPageUrl = "https://github.com/" + Repo;

        static readonly Assembly Exe = typeof(ToolDetails).Assembly;

        // "Mocchi Macro", the product name.
        public static readonly string Name = Exe.GetCustomAttribute<AssemblyProductAttribute>().Product;

        // AssemblyVersion, with all four parts, for comparing with a release's tag.
        public static readonly Version Version = Exe.GetName().Version;

        // "Mocchi Macro 1.0.0", so an organizer asking for help can say which version they're on.
        // Three parts, since the fourth is always 0.
        public static readonly string NameAndVersion = Name + " " + Version.ToString(3);

        // What the tool does, in one line.
        public static readonly string Description = Exe.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;

        // The copyright line, year and author.
        public static readonly string Copyright = Exe.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
    }
}
