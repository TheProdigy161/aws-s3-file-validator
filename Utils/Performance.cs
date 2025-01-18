using System.Diagnostics;

public static class Performance
{
    private static Stopwatch _stopwatch = null;

    public static void Start()
    {
        _stopwatch = Stopwatch.StartNew();
    }

    public static void Stop()
    {
        _stopwatch.Stop();
    }

    public static void Reset()
    {
        _stopwatch.Reset();
    }

    public static string GetTimeTaken()
    {
        // Get time in milliseconds
        long totalMilliseconds = _stopwatch.ElapsedMilliseconds;

        TimeSpan timeSpan = TimeSpan.FromMilliseconds(totalMilliseconds);

        return timeSpan.ToString(@"hh\:mm\:ss\.fff");
    }
}