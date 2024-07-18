using System;
using System.Diagnostics;

public class SimpleStopWatch
{
    public bool IsRunning { get; private set; }
    public TimeSpan ElapsedTime => _stopwatch.Elapsed;

    private Stopwatch _stopwatch;

    public SimpleStopWatch()
    {
        _stopwatch = new Stopwatch();
        IsRunning = false;
    }

    public void Start()
    {
        _stopwatch.Start();
        IsRunning = true;
    }

    public void Stop()
    {
        _stopwatch.Stop();
        IsRunning = false;
    }

    public void Reset()
    {
        _stopwatch.Reset();
        IsRunning = false;
    }
}