// The tool's window: a search text box and a Go button that types the search text into
// the game's monster field, and a typing rate box that sets how many keys per second Go
// clicks. The typing rate is saved in the user settings and restored at startup.
// The search text box holds at most 12 characters and drops any character the keyboard
// has no key for, with a note under it naming what was dropped or saying the text was cut
// to 12. Enter in it runs Go.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MocchiMacro.Properties;

namespace MocchiMacro
{
    public partial class MainWindow : Form
    {
        // The monster field holds at most 12 characters.
        const int MaxSearchTextLength = 12;

        public MainWindow()
        {
            InitializeComponent();

            // Clamped so a hand-edited settings file can't push the box out of its range,
            // which would throw.
            typingRateBox.Value = Math.Max(typingRateBox.Minimum,
                Math.Min(typingRateBox.Maximum, Settings.Default.TypingRate));
        }

        void GoButton_Click(object sender, EventArgs e)
        {
            // The outcome isn't shown yet; the status line comes later.
            new Typist(new User32GamePort()).Type(searchTextBox.Text, (int)typingRateBox.Value);
        }

        // A typed character that has a key, and would make a 13th, is stopped here so it does
        // nothing at all. Pastes don't come through KeyPress, so the cut and its note below
        // only happen for them. Characters without a key go on to be dropped and noted.
        void SearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyboardLayout.HasKey(e.KeyChar)
                && searchTextBox.TextLength - searchTextBox.SelectionLength >= MaxSearchTextLength)
            {
                e.Handled = true;
            }
        }

        // Runs on every edit, typed or pasted, so both are cleaned the same way.
        void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string text = searchTextBox.Text;
            int caret = searchTextBox.SelectionStart;
            char[] dropped = text.Where(c => !KeyboardLayout.HasKey(c)).ToArray();

            // Drop characters without a key, moving the caret back by those before it so it
            // stays where the organizer was typing.
            string kept = new string(text.Where(KeyboardLayout.HasKey).ToArray());
            int keptCaret = caret - text.Take(caret).Count(c => !KeyboardLayout.HasKey(c));

            // The length limit applies after dropping, not through the box's MaxLength, which
            // would cut a paste before its characters without a key are dropped. The caret sits
            // at the end of what was just pasted, so cutting the excess just before it cuts the
            // tail off a long paste.
            int excess = kept.Length - MaxSearchTextLength;
            if (excess > 0)
            {
                keptCaret = Math.Max(0, keptCaret - excess);
                kept = kept.Remove(keptCaret, excess);
            }

            // Setting the text raises this event once more, with nothing to drop, which clears
            // the note; so the note is set after.
            if (kept != text)
            {
                searchTextBox.Text = kept;
                searchTextBox.SelectionStart = keptCaret;
            }
            var note = new List<string>();
            if (dropped.Length > 0)
                note.Add("No key, dropped: " + string.Join(", ", dropped.Distinct().Select(c => c == ' ' ? "space" : c.ToString())));
            if (excess > 0) note.Add("Cut to " + MaxSearchTextLength);
            droppedNoteLabel.Text = string.Join(". ", note);
        }

        void TypingRateBox_ValueChanged(object sender, EventArgs e)
        {
            // The box takes a typed fraction such as 2.5 as its value, while showing it rounded
            // to a whole number. Round the value too, so the rate used is the one shown. Setting
            // it raises this event once more, with the whole number.
            decimal whole = Math.Round(typingRateBox.Value, MidpointRounding.AwayFromZero);
            if (typingRateBox.Value != whole)
            {
                typingRateBox.Value = whole;
                return;
            }

            Settings.Default.TypingRate = (int)whole;
            Settings.Default.Save();
        }
    }
}
