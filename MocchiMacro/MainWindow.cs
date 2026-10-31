// The tool's window: a search text box and a Go button that types the search text into
// the game's monster field, and a typing rate box that sets how many keys per second Go
// clicks. The typing rate is saved in the user settings when the window closes and restored
// at startup.
// The search text box swaps stand-ins such as '@' for the symbol keys they stand for, holds
// at most 12 characters and drops any character the keyboard has no key for, with a note
// under it naming what was dropped or saying the text was cut to 12. Its tooltip lists the
// stand-ins. Enter in it runs Go. While Go types, it and both boxes are disabled; then a status
// line between the search text and the typing rate says how typing went.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
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

            // Lists the stand-ins, such as "@ ○", so the organizer can find them by hovering.
            searchTextToolTip.SetToolTip(searchTextBox,
                string.Join("   ", KeyboardLayout.StandIns.Select(s => s.StandIn + " " + s.Symbol)));
        }

        async void GoButton_Click(object sender, EventArgs e)
        {
            // With nothing to type, Go would only clear the monster field.
            if (searchTextBox.TextLength == 0)
            {
                ShowStatus("Type a search text first.", ProblemColor);
                return;
            }

            string searchText = searchTextBox.Text;
            int typingRate = (int)typingRateBox.Value;

            // Disabled so a second Go can't start typing on top of the first.
            SetInputsEnabled(false);
            ShowStatus("Typing...", SystemColors.ControlText);
            try
            {
                // Typing takes seconds, so it runs off the UI thread to keep the window responsive.
                TypingOutcome outcome = await Task.Run(
                    () => new Typist(new User32GamePort()).Type(searchText, typingRate));
                ShowStatus(DescribeOutcome(outcome), outcome == TypingOutcome.Done ? DoneColor : ProblemColor);
            }
            finally
            {
                SetInputsEnabled(true);
            }
        }

        // Problems use the dropped-characters note's red, so red means one thing across the window.
        // Done is a dark green, about as easy to read on the window's grey as that red.
        Color ProblemColor => droppedNoteLabel.ForeColor;
        static readonly Color DoneColor = Color.DarkGreen;

        void ShowStatus(string text, Color color)
        {
            statusLabel.Text = text;
            statusLabel.ForeColor = color;
        }

        void SetInputsEnabled(bool enabled)
        {
            goButton.Enabled = enabled;
            searchTextBox.Enabled = enabled;
            typingRateBox.Enabled = enabled;
        }

        static string DescribeOutcome(TypingOutcome outcome)
        {
            switch (outcome)
            {
                case TypingOutcome.Done: return "Done. Check the monster field, then press Ent. in the game.";
                case TypingOutcome.GameNotFound: return "Game not found. Start MR2 DX, then press Go again.";
                case TypingOutcome.CharacterWithoutKey: return "The search text has a character without a key.";
                case TypingOutcome.GameNotInFront: return "The game didn't come to the front. Click the game, then press Go again.";
                case TypingOutcome.GameClosed: return "The game window closed partway through, so typing stopped.";
                default: return outcome.ToString();
            }
        }

        // A typed character that has a key, and would make a 13th, is stopped here so it does
        // nothing at all. Pastes don't come through KeyPress, so the cut and its note below
        // only happen for them. Characters without a key go on to be dropped and noted. A
        // stand-in counts as its symbol key, so it's stopped too.
        void SearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyboardLayout.HasKey(KeyboardLayout.SwapStandIn(e.KeyChar))
                && searchTextBox.TextLength - searchTextBox.SelectionLength >= MaxSearchTextLength)
            {
                e.Handled = true;
            }
        }

        // Runs on every edit, typed or pasted, so both are cleaned the same way.
        void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            // Stand-ins are swapped for their symbol keys first, so they're kept and counted
            // toward the 12 like any other key. The swap is one for one, so the caret stays put.
            string text = new string(searchTextBox.Text.Select(KeyboardLayout.SwapStandIn).ToArray());
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

            // Compared with the box's own text, not the swapped text, so a swap alone is written
            // back. Setting the text raises this event once more, with nothing to swap or drop,
            // which clears the note; so the note is set after.
            if (kept != searchTextBox.Text)
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
        }

        // Saved once on close, not on every change: each save rewrites user.config, and a quick
        // run of arrow clicks rewrote it so fast that anti-ransomware software took the tool for
        // ransomware and suspended it.
        void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.Save();
        }
    }
}
