// Update check tests: whether a release's tag is newer than the running version, which decides
// whether the update notice shows. No network: the check's request isn't tested here.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MocchiMacro.Tests
{
    [TestClass]
    public class UpdateCheckTests
    {
        // The running version as AssemblyVersion gives it, with all four parts.
        static readonly Version Running = new Version(1, 0, 0, 0);

        [TestMethod]
        public void ANewerTagIsTheNewerRelease()
        {
            Assert.AreEqual(new Version(1, 1, 0), UpdateCheck.NewerRelease("1.1.0", Running));
        }

        [TestMethod]
        public void AnEqualTagIsNoNewerRelease()
        {
            Assert.IsNull(UpdateCheck.NewerRelease("1.0.0", Running));
        }

        [TestMethod]
        public void AnOlderTagIsNoNewerRelease()
        {
            Assert.IsNull(UpdateCheck.NewerRelease("0.9.0", Running));
        }

        [TestMethod]
        public void AnUnreadableTagIsNoNewerRelease()
        {
            Assert.IsNull(UpdateCheck.NewerRelease("latest", Running));
            Assert.IsNull(UpdateCheck.NewerRelease("", Running));
            Assert.IsNull(UpdateCheck.NewerRelease(null, Running));
        }

        // Tags are plain versions; a leading v isn't stripped, so such a release shows no notice.
        [TestMethod]
        public void AVTagIsNoNewerRelease()
        {
            Assert.IsNull(UpdateCheck.NewerRelease("v1.1.0", Running));
        }

        // Version counts a missing fourth part as lower than .0, so without cutting both to three
        // parts, 1.1.0 would come out older than 1.1.0.0.
        [TestMethod]
        public void AThreePartTagEqualsTheSameFourPartVersion()
        {
            Assert.IsNull(UpdateCheck.NewerRelease("1.1.0", new Version(1, 1, 0, 0)));
            Assert.AreEqual(new Version(1, 1, 0), UpdateCheck.NewerRelease("1.1.0", new Version(1, 0, 9, 0)));
        }
    }
}
