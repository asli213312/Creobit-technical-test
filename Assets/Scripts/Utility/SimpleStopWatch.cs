using System;
using System.Diagnostics;

public class SimpleStopWatch
{
    #region Properties
    public bool IsRunning { get; private set; }
    public TimeSpan ElapsedTime => _stopwatch.Elapsed;

    #endregion Properties

    #region Private Fields
    private Stopwatch _stopwatch;

    #endregion Private Fields

    #region Public Methods
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

    #endregion Public Methods
}