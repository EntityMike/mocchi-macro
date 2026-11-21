// The tool's window: a search text box and a Go button that types the search text into
// the game's monster field, a typing rate box that sets how many keys per second Go clicks,
// and a game dropdown that picks which game Go types into, MR1 DX or MR2 DX. The typing rate
// and the game are saved in the user settings when the window closes and restored at startup.
// Switching games clears the search text box.
// The search text box swaps stand-ins such as '@' for the symbol keys they stand for, holds
// at most 12 characters and drops any character the selected game's keyboard has no key for,
// with a note under it naming what was dropped or saying the text was cut to 12. Its tooltip
// lists the stand-ins you can type in the selected game. Enter in it runs Go. While Go types,
// it, both boxes and the dropdown are disabled; then a status line between the search text
// and the typing rate says how typing went.
// The hotkey brings the window to the front from any program with the search text selected,
// so the organizer can type the next one over it. It's Ctrl+Alt+Shift+M, or a fallback when
// another program already has that, or none when all three are taken; a label at the bottom
// shows which, and the status line says so at startup when it isn't the default.
// The mascot stands to the left of the controls and reacts to Go: it curls in a loop while Go
// types, then hops if the outcome is Done and droops on any problem, including an empty box.
// Startup messages don't make it droop, since that's only about Go.
// The title bar shows the tool's version, such as "Mocchi Macro 1.0.0".
// A footer row along the bottom shows the footer band, a still strip of pixel-art grass.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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

            // Set here, after InitializeComponent, not in the designer. A form font set while
            // WinForms still has scaling to do makes the fixed-size labels scale twice above 100%,
            // and the oversized status line then covers the game dropdown. The designer's view
            // shows the default font as a result.
            Font = new Font("Segoe UI", 9F);

            // "Mocchi Macro 1.0.0", so an organizer asking for help can say which version they're
            // on. Read from AssemblyVersion rather than written out, so a release only bumps the
            // version in AssemblyInfo.cs. Three parts, since the fourth is always 0.
            Text = "Mocchi Macro " + typeof(MainWindow).Assembly.GetName().Version.ToString(3);

            // For the title bar, taskbar and Alt+Tab. The .exe's own icon doesn't reach these:
            // without this the window shows WinForms' default icon.
            Icon = MascotArt.LoadIcon();

            // Clamped so a hand-edited settings file can't push the box out of its range,
            // which would throw.
            typingRateBox.Value = Math.Max(typingRateBox.Minimum,
                Math.Min(typingRateBox.Maximum, Settings.Default.TypingRate));

            // Most organizers play MR2 DX, so it's the default, and it also stands in for a saved
            // game that isn't one of ours, such as a hand-edited one, rather than throwing.
            // Selecting the game also sets the tooltip, below.
            gameBox.Items.AddRange(Game.All.ToArray());
            gameBox.SelectedItem = Game.All.FirstOrDefault(g => g.Name == Settings.Default.Game) ?? Game.Mr2Dx;

            LayOutAroundMascot();
        }

        // By now WinForms has scaled the designer's layout for the display, but the mascot keeps
        // its fixed size at every scaling. So the mascot's area takes the frame's real size here,
        // and the controls and the window follow it to keep the margins even.
        // That scaling is AutoScaleMode.Dpi, not the usual Font, which left the controls too small
        // for Segoe UI's text above 100%.
        void LayOutAroundMascot()
        {
            int margin = mascotBox.Left;
            ShowMascotFrame();
            controlsColumn.Left = mascotBox.Right + margin;
            ClientSize = new Size(controlsColumn.Right + margin, ClientSize.Height);
            mascotBox.Top = (footerRow.Top - mascotBox.Height) / 2;
            ShowFooterBand();
        }

        // The footer row is docked along the window's bottom, so it grows with the window at
        // higher scaling (about 27 px tall at 150%), while the band, like the mascot, stays 18 px.
        // The band sits on the row's bottom edge, so the grass meets the bottom of the window.
        // It's built once, as wide as the row, now the window has its final width.
        void ShowFooterBand()
        {
            footerBand.Image = FooterBand.Build(footerRow.Width);
            footerBand.Bounds = new Rectangle(0, footerRow.Height - footerBand.Image.Height,
                footerRow.Width, footerBand.Image.Height);
        }

        readonly MascotAnimation mascotAnimation = new MascotAnimation();

        // Starts with the animation, whose idle counts from time 0.
        readonly Stopwatch mascotClock = Stopwatch.StartNew();

        // Each frame is enlarged once and kept, rather than again on every showing.
        readonly Dictionary<string, Bitmap> mascotFrames = new Dictionary<string, Bitmap>();

        long MascotNowMs => mascotClock.ElapsedMilliseconds;

        void MascotTimer_Tick(object sender, EventArgs e)
        {
            ShowMascotFrame();
        }

        // Most ticks land on the frame already showing, and those don't repaint.
        void ShowMascotFrame()
        {
            string name = mascotAnimation.FrameAt(MascotNowMs);
            if (!mascotFrames.TryGetValue(name, out Bitmap frame))
            {
                frame = MascotArt.LoadFrame(name);
                mascotFrames[name] = frame;
            }
            if (mascotBox.Image != frame) mascotBox.Image = frame;
        }

        Game SelectedGame => (Game)gameBox.SelectedItem;

        // So only a real switch clears the search text box.
        Game lastSelectedGame;

        void GameBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // The dropdown can raise this without the game changing, such as when the organizer
            // picks the game that's already selected; that mustn't clear the search text box.
            if (SelectedGame == lastSelectedGame) return;
            lastSelectedGame = SelectedGame;

            // Saved with the typing rate, on close.
            Settings.Default.Game = SelectedGame.Name;

            // A switch means starting over with the other game's monster names, so the old text
            // is cleared rather than left to be sent to the wrong game. The note is cleared here
            // too, not left to TextChanged: a box already emptied by dropping raises no
            // TextChanged on Clear, and would keep its note. The status line stays.
            searchTextBox.Clear();
            droppedNoteLabel.Text = "";

            // Lists the selected game's stand-ins, such as "@ ○", so the organizer can find them
            // by hovering. Only the ASCII ones: the Japanese middle dot isn't on an ordinary
            // keyboard, only pasted.
            searchTextToolTip.SetToolTip(searchTextBox, string.Join("   ", SelectedGame.Layout.StandIns
                .Where(s => s.StandIn < 128).Select(s => s.StandIn + " " + s.Symbol)));
        }

        async void GoButton_Click(object sender, EventArgs e)
        {
            // With nothing to type, Go would only clear the monster field.
            if (searchTextBox.TextLength == 0)
            {
                ShowStatus("Type a search text first.", ProblemColor);
                mascotAnimation.PlayProblem(MascotNowMs);
                return;
            }

            string searchText = searchTextBox.Text;
            int typingRate = (int)typingRateBox.Value;
            Game game = SelectedGame;

            // Disabled so a second Go can't start typing on top of the first.
            SetInputsEnabled(false);
            ShowStatus("Typing...", SystemColors.ControlText);
            mascotAnimation.PlayTyping(MascotNowMs);
            try
            {
                // Typing takes seconds, so it runs off the UI thread to keep the window responsive.
                TypingOutcome outcome = await Task.Run(
                    () => new Typist(new User32GamePort(game.WindowTitle), game.Layout).Type(searchText, typingRate));
                string description = DescribeOutcome(outcome, game, searchText);
                if (outcome == TypingOutcome.Done)
                {
                    ShowStatus(description, DoneColor);
                    mascotAnimation.PlayDone(MascotNowMs);
                }
                else
                {
                    ShowStatus(description, ProblemColor);
                    mascotAnimation.PlayProblem(MascotNowMs);
                }
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
            gameBox.Enabled = enabled;
        }

        static string DescribeOutcome(TypingOutcome outcome, Game game, string searchText)
        {
            switch (outcome)
            {
                case TypingOutcome.Done:
                    // The 12th character moves the game on by itself, as Ent. does, so by then
                    // there's no monster field to check and pressing the key would hit the next screen.
                    return searchText.Length == MaxSearchTextLength
                        ? "Done. The game has moved on to the next screen."
                        : "Done. Check the monster field, then press " + game.EntKeyLabel + " in the game.";
                // Names the game, so an organizer who picked the wrong one can tell.
                case TypingOutcome.GameNotFound: return game.Name + " not found. Start it, then press Go again.";
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
            KeyboardLayout layout = SelectedGame.Layout;
            if (layout.HasKey(layout.SwapStandIn(e.KeyChar))
                && searchTextBox.TextLength - searchTextBox.SelectionLength >= MaxSearchTextLength)
            {
                e.Handled = true;
            }
        }

        // Runs on every edit, typed or pasted, so both are cleaned the same way.
        void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            KeyboardLayout layout = SelectedGame.Layout;

            // Stand-ins are swapped for their symbol keys first, so they're kept and counted
            // toward the 12 like any other key. The swap is one for one, so the caret stays put.
            string text = new string(searchTextBox.Text.Select(layout.SwapStandIn).ToArray());
            int caret = searchTextBox.SelectionStart;
            char[] dropped = text.Where(c => !layout.HasKey(c)).ToArray();

            // Drop characters without a key, moving the caret back by those before it so it
            // stays where the organizer was typing.
            string kept = new string(text.Where(layout.HasKey).ToArray());
            int keptCaret = caret - text.Take(caret).Count(c => !layout.HasKey(c));

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
            // The label has UseMnemonic off, so a dropped & shows as itself rather than as an
            // underline on the next character.
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

        // Identifies the hotkey in WM_HOTKEY. Any number works, since the window has only one.
        const int HotkeyId = 1;

        // The hotkey's combinations, tried in order; the first no other program has is kept.
        // Not Ctrl+Shift+M: other programs often take it, and most Ctrl+Shift+letters, by default.
        // The global-hotkey spec says why each of these was picked.
        static readonly (string Name, uint Modifiers, Keys Key)[] HotkeyCombinations =
        {
            ("Ctrl+Alt+Shift+M", MOD_CONTROL | MOD_ALT | MOD_SHIFT, Keys.M),
            ("Ctrl+Shift+F9", MOD_CONTROL | MOD_SHIFT, Keys.F9),
            ("Ctrl+Alt+Shift+F9", MOD_CONTROL | MOD_ALT | MOD_SHIFT, Keys.F9),
        };

        // Registered with the window's handle and released with it, so the combination reaches
        // other programs again once the tool closes. Tried once, at startup: one that another
        // program lets go of later isn't picked up. No auto-repeat, so holding the keys fires once.
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            string hotkeyName = null;
            foreach (var combination in HotkeyCombinations)
            {
                if (RegisterHotKey(Handle, HotkeyId, combination.Modifiers | MOD_NOREPEAT, (uint)combination.Key))
                {
                    hotkeyName = combination.Name;
                    break;
                }
            }

            hotkeyLabel.Text = "Hotkey: " + (hotkeyName ?? "none");

            // Said once, so the organizer isn't left pressing a combination that does nothing.
            // The first Go replaces it with its own status. The default needs nothing beyond the label.
            string defaultHotkey = HotkeyCombinations[0].Name;
            if (hotkeyName == null)
                ShowStatus("All three hotkeys are taken by other programs, so there's no hotkey. Typing still works.", ProblemColor);
            else if (hotkeyName != defaultHotkey)
                ShowStatus(defaultHotkey + " is taken by another program, so the hotkey is " + hotkeyName + ".", ProblemColor);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            UnregisterHotKey(Handle, HotkeyId);
            base.OnHandleDestroyed(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY && m.WParam == (IntPtr)HotkeyId) BringUpSearchText();
            base.WndProc(ref m);
        }

        void BringUpSearchText()
        {
            // The search text box is disabled while Go types. Taking focus then would pull the
            // game out of the front partway through a name.
            if (!searchTextBox.Enabled) return;

            if (WindowState == FormWindowState.Minimized) WindowState = FormWindowState.Normal;

            // Windows normally stops a background program from taking the front, but the
            // program that receives a hotkey is allowed to: the key press was meant for it.
            Activate();
            searchTextBox.Focus();
            searchTextBox.SelectAll();
        }

        // ---- Win32 interop (user32.dll) ----

        const int WM_HOTKEY = 0x0312;
        const uint MOD_ALT = 0x0001;
        const uint MOD_CONTROL = 0x0002;
        const uint MOD_SHIFT = 0x0004;
        const uint MOD_NOREPEAT = 0x4000;

        [DllImport("user32.dll")] static extern bool RegisterHotKey(IntPtr hwnd, int id, uint modifiers, uint key);
        [DllImport("user32.dll")] static extern bool UnregisterHotKey(IntPtr hwnd, int id);
    }
}
