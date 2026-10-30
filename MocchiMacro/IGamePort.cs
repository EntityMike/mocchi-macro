// The typist's only way to reach the game: the one seam the tests replace with a fake.
// The real version is User32GamePort.

using System.Drawing;

namespace MocchiMacro
{
    public interface IGamePort
    {
        // Looks for the game window and remembers it for the calls below.
        bool FindGame();

        // Whether the window found is still open.
        bool GameStillExists();

        // Whether the game window is minimized, and restoring it to its normal size.
        bool IsMinimized();
        void Restore();

        // Asks Windows to put the game window in front. Windows may refuse or take a while,
        // so whether it worked is read back separately.
        void BringToFront();
        bool IsInFront();

        // The game window's client area: its size, and its top-left corner on screen.
        Rectangle ReadClientArea();

        // Clicks a point on screen with the left mouse button.
        void Click(Point screenPoint);

        // Milliseconds since some fixed point; only differences matter.
        long NowMs { get; }

        void Wait(int ms);
    }
}
