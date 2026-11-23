// Changelog tests: CHANGELOG.md's newest entry is for the version the tool is built as. Bumping
// AssemblyVersion for a release without adding its changelog entry, or the other way round,
// fails here instead of shipping.

using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MocchiMacro.Tests
{
    [TestClass]
    public class ChangelogTests
    {
        // Entries are "## 1.0.0" headings, newest first. Three parts, since AssemblyVersion's
        // fourth is always 0 and release tags leave it off.
        [TestMethod]
        public void TheNewestEntryIsTheToolsVersion()
        {
            Version version = typeof(ToolDetails).Assembly.GetName().Version;
            string expected = "## " + version.ToString(3);

            string testsFolder = Path.GetDirectoryName(typeof(ChangelogTests).Assembly.Location);
            string newestEntry = File.ReadLines(Path.Combine(testsFolder, "CHANGELOG.md"))
                .First(line => line.StartsWith("## "));

            Assert.AreEqual(expected, newestEntry.Trim());
        }
    }
}
