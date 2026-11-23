// The About box: a small fixed-size window, opened by the "?" button in the tool's window, saying
// what the tool is, who made it, and where to get help. The trophy frame, Mocchi sitting in a
// giant gold cup, stands on the left; on the right are the name and version, what the tool does, the copyright and
// license, links to help, support, and the game, thanks, the not-affiliated line, and "Enjoy!".
// The name, description, and copyright come from the .exe's file details through ToolDetails.
// Nothing is fetched: a link only opens the default browser when clicked. OK, Enter, and Esc
// close it.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace MocchiMacro
{
    public partial class AboutBox : Form
    {
        // The repo's page shows the README, which links to the changelog.
        const string HelpUrl = ToolDetails.RepoPageUrl;
        const string SupportUrl = "https://discord.gg/8bgNyFBvHp";
        const string SteamUrl = "https://store.steampowered.com/app/1716120/Monster_Rancher_1__2_DX/";

        // The trophy frame, made for this box, enlarged once and kept for every opening, as
        // MainWindow keeps its frames.
        static readonly Bitmap TrophyFrame = MascotArt.LoadFrame("about-1");

        public AboutBox()
        {
            InitializeComponent();

            // After InitializeComponent, as in MainWindow, which says what broke otherwise.
            Font = new Font("Segoe UI", 9F);
            nameAndVersionLabel.Font = new Font(Font, FontStyle.Bold);

            Text = "About " + ToolDetails.Name;
            nameAndVersionLabel.Text = ToolDetails.NameAndVersion;
            descriptionLabel.Text = ToolDetails.Description + ".";
            copyrightLabel.Text = ToolDetails.Copyright + " · MIT license";

            helpLink.Links[0].LinkData = HelpUrl;
            supportLink.Links[0].LinkData = SupportUrl;
            steamLink.Links[0].LinkData = SteamUrl;

            // Focus starts on OK, not the first link, so Enter closes the box rather than
            // opening the browser.
            ActiveControl = okButton;

            LayOutAroundMascot();
        }

        // As in MainWindow: WinForms has scaled the designer's layout for the display, but the
        // mascot keeps its fixed 3× size, so the text and the window follow it. The text column
        // sizes itself to its text in the font just set, so nothing is cut off at any scaling.
        void LayOutAroundMascot()
        {
            int margin = mascotBox.Left;
            mascotBox.Image = TrophyFrame;
            textColumn.Left = mascotBox.Right + margin;
            ClientSize = new Size(textColumn.Right + margin,
                Math.Max(mascotBox.Bottom, textColumn.Bottom) + margin);
        }

        void Link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Without a default browser this throws; nothing happens instead, as with the
            // update notice.
            try
            {
                Process.Start((string)e.Link.LinkData);
            }
            catch (Exception)
            {
            }
        }
    }
}
