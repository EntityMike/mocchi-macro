// The Monster Rancher DX games the tool can type into: MR1 DX and MR2 DX. Each has the name
// shown in the main window's game dropdown, the title of its game window, and its keyboard
// layout.

using System.Collections.Generic;

namespace MocchiMacro
{
    public class Game
    {
        public static readonly Game Mr1Dx = new Game("MR1 DX", "MF", KeyboardLayout.Mr1Dx);
        public static readonly Game Mr2Dx = new Game("MR2 DX", "MF2", KeyboardLayout.Mr2Dx);

        // In the dropdown's order.
        public static readonly IReadOnlyList<Game> All = new[] { Mr1Dx, Mr2Dx };

        public string Name { get; }
        public string WindowTitle { get; }
        public KeyboardLayout Layout { get; }

        Game(string name, string windowTitle, KeyboardLayout layout)
        {
            Name = name;
            WindowTitle = windowTitle;
            Layout = layout;
        }

        // The dropdown shows each game by this.
        public override string ToString() => Name;
    }
}
