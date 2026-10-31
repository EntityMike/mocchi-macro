// Keyboard layout tests, for each game: which characters have a key, for the cases the search
// text box relies on to drop characters the keyboard can't type, and which stand-ins it swaps
// for the symbol keys.

using System.Linq;
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
            Assert.IsTrue(KeyboardLayout.Mr2Dx.HasKey(c));
        }

        [TestMethod]
        [DataRow(' ')]
        [DataRow('é')]
        [DataRow('Ü')]
        [DataRow(',')]
        [DataRow('\0')]
        public void HasNoKeyFor(char c)
        {
            Assert.IsFalse(KeyboardLayout.Mr2Dx.HasKey(c));
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
            Assert.AreEqual(symbol, KeyboardLayout.Mr2Dx.SwapStandIn(standIn));
            Assert.IsTrue(KeyboardLayout.Mr2Dx.HasKey(symbol));
            Assert.IsFalse(KeyboardLayout.Mr2Dx.HasKey(standIn));
        }

        [TestMethod]
        [DataRow('A')]
        [DataRow('Z')]
        [DataRow('a')]
        [DataRow('z')]
        [DataRow('0')]
        [DataRow('9')]
        [DataRow('○')] // ○
        [DataRow('×')] // ×
        [DataRow('_')]
        [DataRow('/')]
        [DataRow('·')] // ·
        [DataRow('~')]
        [DataRow('%')]
        [DataRow('!')]
        [DataRow('?')]
        [DataRow('+')]
        [DataRow('-')]
        [DataRow('.')]
        public void Mr1DxHasKeyFor(char c)
        {
            Assert.IsTrue(KeyboardLayout.Mr1Dx.HasKey(c));
        }

        [TestMethod]
        [DataRow('△')] // △
        [DataRow('□')] // □
        [DataRow('(')]
        [DataRow(')')]
        [DataRow('\'')]
        [DataRow('"')]
        [DataRow(':')]
        [DataRow('=')]
        [DataRow(' ')]
        [DataRow('é')]
        [DataRow('Ü')]
        [DataRow('←')] // ←, which is BS, not a character
        public void Mr1DxHasNoKeyFor(char c)
        {
            Assert.IsFalse(KeyboardLayout.Mr1Dx.HasKey(c));
        }

        [TestMethod]
        [DataRow('@', '○')] // ○
        [DataRow('*', '×')] // ×
        [DataRow('`', '·')] // ·
        [DataRow('・', '·')] // ·
        public void Mr1DxStandInSwapsForItsSymbolKey(char standIn, char symbol)
        {
            Assert.AreEqual(symbol, KeyboardLayout.Mr1Dx.SwapStandIn(standIn));
            Assert.IsTrue(KeyboardLayout.Mr1Dx.HasKey(symbol));
            Assert.IsFalse(KeyboardLayout.Mr1Dx.HasKey(standIn));
        }

        // MR1 DX has no △ or □, so their stand-ins stay as they are, to be dropped by the box.
        [TestMethod]
        [DataRow('^')]
        [DataRow('#')]
        public void Mr1DxLeavesTheStandInsForSymbolsItLacksAsCharactersWithoutAKey(char c)
        {
            Assert.AreEqual(c, KeyboardLayout.Mr1Dx.SwapStandIn(c));
            Assert.IsFalse(KeyboardLayout.Mr1Dx.HasKey(c));
        }

        // The box's tooltip lists these in this order.
        [TestMethod]
        public void EachGameListsTheStandInsForTheSymbolsItHas()
        {
            CollectionAssert.AreEqual(new[] { '@', '*', '`', '\u30FB' },
                KeyboardLayout.Mr1Dx.StandIns.Select(s => s.StandIn).ToArray());
            CollectionAssert.AreEqual(new[] { '@', '*', '^', '#', '`', '\u30FB' },
                KeyboardLayout.Mr2Dx.StandIns.Select(s => s.StandIn).ToArray());
        }
    }
}
