using System;
using System.Diagnostics;

namespace LFE.KeyboardShortcuts.Utils
{
    public class TimingLogger : IDisposable
    {
        private readonly string _identifier;
        private readonly Stopwatch _stopwatch;

        private TimingLogger(string identifier)
        {
            _identifier = identifier;
            _stopwatch = Stopwatch.StartNew();
        }

        public static TimingLogger Track(string identifier)
        {
            return new TimingLogger(identifier);
        }

        public void Log(string additional = null)
        {
            SuperController.LogMessage($"{_identifier} {additional} [{_stopwatch.ElapsedMilliseconds}ms]");
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            Log();
        }
    }
}
