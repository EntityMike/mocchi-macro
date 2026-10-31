// Keyboard layout tests: which characters have a key, for the cases the search text box
// relies on to drop characters the keyboard can't type, and which stand-ins it swaps for
// the symbol keys.

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
        [DataRow('\u00B7')] // ·
        public void HasKeyFor(char c)
        {
            Assert.IsTrue(KeyboardLayout.HasKey(c));
        }

        [TestMethod]
        [DataRow(' ')]
        [DataRow('é')]
        [DataRow('Ü')]
        [DataRow(',')]
        [DataRow('\0')]
        public void HasNoKeyFor(char c)
        {
            Assert.IsFalse(KeyboardLayout.HasKey(c));
        }

        [TestMethod]
        [DataRow('@', '\u25CB')] // ○
        [DataRow('*', '\u00D7')] // ×
        [DataRow('^', '\u25B3')] // △
        [DataRow('#', '\u25A1')] // □
        [DataRow('`', '\u00B7')] // ·
        [DataRow('\u30FB', '\u00B7')] // ·
        public void StandInSwapsForItsSymbolKey(char standIn, char symbol)
        {
            Assert.AreEqual(symbol, KeyboardLayout.SwapStandIn(standIn));
            Assert.IsTrue(KeyboardLayout.HasKey(symbol));
            Assert.IsFalse(KeyboardLayout.HasKey(standIn));
        }
    }
}
