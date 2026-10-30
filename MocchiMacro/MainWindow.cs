// The tool's window: a search text box and a Go button that types the search text into
// the game's monster field.

using System;
using System.Windows.Forms;

namespace MocchiMacro
{
    public partial class MainWindow : Form
    {
        // Fixed for now; the typing rate box replaces it.
        const int TypingRate = 10;

        public MainWindow()
        {
            InitializeComponent();
        }

        void GoButton_Click(object sender, EventArgs e)
        {
            // The outcome isn't shown yet; the status line comes later.
            new Typist(new User32GamePort()).Type(searchTextBox.Text, TypingRate);
        }
    }
}
