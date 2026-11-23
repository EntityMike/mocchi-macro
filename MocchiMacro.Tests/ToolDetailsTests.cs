// Tool details tests: the name, version, description, and copyright the title bar and the About
// box show match the .exe's file details, as Windows reads them from its version resource for
// Properties → Details. So nothing the tool shows can drift from what the file says.

using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MocchiMacro.Tests
{
    [TestClass]
    public class ToolDetailsTests
    {
        static readonly FileVersionInfo FileDetails =
            FileVersionInfo.GetVersionInfo(typeof(ToolDetails).Assembly.Location);

        [TestMethod]
        public void TheNameIsTheProductName()
        {
            Assert.AreEqual(FileDetails.ProductName, ToolDetails.Name);
        }

        // Three parts, since the fourth is always 0.
        [TestMethod]
        public void TheNameAndVersionIsTheProductNameAndItsThreePartVersion()
        {
            string expected = FileDetails.ProductName + " " + FileDetails.ProductMajorPart + "."
                + FileDetails.ProductMinorPart + "." + FileDetails.ProductBuildPart;
            Assert.AreEqual(expected, ToolDetails.NameAndVersion);
        }

        // Properties → Details doesn't show it on Windows 10 and 11, but the version resource has it.
        [TestMethod]
        public void TheDescriptionIsTheFilesComments()
        {
            Assert.AreEqual(FileDetails.Comments, ToolDetails.Description);
        }

        [TestMethod]
        public void TheCopyrightIsTheFilesCopyright()
        {
            Assert.AreEqual(FileDetails.LegalCopyright, ToolDetails.Copyright);
        }
    }
}
