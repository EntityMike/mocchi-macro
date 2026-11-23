// Decides which mascot frame shows at any moment. It knows nothing of windows or clocks: the
// window passes in the time and draws the frame named back. Working from the time rather than
// stepping frame by frame means a late timer tick can't slow the animation down.
// Idle and typing loop. Done and problem play once and hold their last frame, then idle resumes
// by itself. A new Play cuts short whatever is showing.

using System.Linq;

namespace MocchiMacro
{
    public class MascotAnimation
    {
        // Each animation's frames and how long each shows, copied from the "play" line of its
        // file in art/. Change them together: a test fails if they differ.
        static readonly Animation Idle = new Animation(loops: true,
            ("idle-1", 3000), ("idle-2", 60), ("idle-3", 100), ("idle-2", 60));

        static readonly Animation Typing = new Animation(loops: true,
            ("typing-1", 200), ("typing-2", 90), ("typing-3", 90), ("typing-4", 240), ("typing-3", 90), ("typing-2", 90));

        static readonly Animation Done = new Animation(loops: false,
            ("done-1", 80), ("done-2", 200), ("done-1", 80), ("done-3", 150));

        static readonly Animation Problem = new Animation(loops: false,
            ("problem-1", 150), ("problem-2", 150), ("problem-3", 200));

        // How long a once-only animation's last frame stays, on top of its own time in the list.
        const int HoldMs = 2000;

        Animation playing = Idle;
        long startedMs;

        public void PlayTyping(long nowMs) => Play(Typing, nowMs);
        public void PlayDone(long nowMs) => Play(Done, nowMs);
        public void PlayProblem(long nowMs) => Play(Problem, nowMs);

        void Play(Animation animation, long nowMs)
        {
            playing = animation;
            startedMs = nowMs;
        }

        public string FrameAt(long nowMs)
        {
            long elapsedMs = nowMs - startedMs;
            long idleResumesMs = playing.LengthMs + HoldMs;
            return playing.Loops || elapsedMs < idleResumesMs
                ? playing.FrameAfter(elapsedMs)
                : Idle.FrameAfter(elapsedMs - idleResumesMs);
        }

        class Animation
        {
            readonly (string Frame, int Ms)[] steps;

            public Animation(bool loops, params (string Frame, int Ms)[] steps)
            {
                Loops = loops;
                this.steps = steps;
                LengthMs = steps.Sum(step => step.Ms);
            }

            public bool Loops { get; }
            public int LengthMs { get; }

            // Past the end, a loop comes round again and anything else stays on its last frame,
            // which is the hold.
            public string FrameAfter(long elapsedMs)
            {
                if (Loops)
                {
                    elapsedMs %= LengthMs;
                }

                foreach ((string Frame, int Ms) step in steps)
                {
                    if (elapsedMs < step.Ms)
                    {
                        return step.Frame;
                    }

                    elapsedMs -= step.Ms;
                }
                return steps.Last().Frame;
            }
        }
    }
}
