// Keyboard layout tests: which characters have a key, for the cases the search text box
// relies on to drop characters the keyboard can't type.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MocchiMacro.Tests
{
    [TestClass]
    public class KeyboardLayoutTests
    {
        [TestMethod]
        [DataRow('A')]
        [DataRow('Z')]
        [DataRow('a')]
        [DataRow('z')]
        [DataRow('0')]
        [DataRow('9')]
        [DataRow('\u25CB')] // ○
        [DataRow('\u00D7')] // ×
        [DataRow('\u25B3')] // △
        [DataRow('\u25A1')] // □
        public void HasKeyFor(char c)
        {
            Assert.IsTrue(KeyboardLayout.HasKey(c));
        }

        [TestMethod]
        [DataRow(' ')]
        [DataRow('é')]
        [DataRow('Ü')]
        // The unknown punctuation key looks like a comma or a raised dot, and is left out.
        [DataRow(',')]
        [DataRow('\u00B7')] // ·
        [DataRow('\0')]
        public void HasNoKeyFor(char c)
        {
            Assert.IsFalse(KeyboardLayout.HasKey(c));
        }
    }
}
