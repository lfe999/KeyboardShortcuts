using System;
using System.Diagnostics;
using System.Linq;
using LFE.KeyboardShortcuts.Extensions;

namespace LFE.KeyboardShortcuts.Utils
{
    public class TimingLogger : IDisposable
    {
        private static readonly UnityEngine.UI.Toggle PERF_MON = UserPreferences.singleton.transform
            .Find((p) => p.EndsWith("PerfMon Toggle"))
            .FirstOrDefault()
            ?.GetComponent<UnityEngine.UI.Toggle>();
        private readonly string _identifier;
        private readonly Stopwatch _stopwatch;
        private bool _shouldLog;

        private TimingLogger(string identifier)
        {
            _identifier = identifier;
            _stopwatch = Stopwatch.StartNew();
            _shouldLog = PERF_MON?.isOn ?? false;
        }

        public static TimingLogger Track(string identifier)
        {
            return new TimingLogger(identifier);
        }

        public void Log(string additional = null)
        {
            if(_shouldLog) { SuperController.LogMessage($"{_identifier} {additional} [{_stopwatch.ElapsedMilliseconds}ms]", false); }
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            Log();
        }
    }
}
