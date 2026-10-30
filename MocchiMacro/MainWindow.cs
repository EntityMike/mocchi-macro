// The tool's window: a search text box and a Go button that types the search text into
// the game's monster field, and a typing rate box that sets how many keys per second Go
// clicks. The typing rate is saved in the user settings and restored at startup.

using System;
using System.Windows.Forms;
using MocchiMacro.Properties;

namespace MocchiMacro
{
    public partial class MainWindow : Form
    {
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
