using System.Diagnostics;

namespace aws_s3_file_validator.Utils;

[AttributeUsage(AttributeTargets.Method)]
public class PerformanceAttribute : Attribute, IDisposable
{
    private Stopwatch _stopwatch;

    public void Execute()
    {
        _stopwatch = Stopwatch.StartNew();

        Console.WriteLine("Starting the method...");
    }

    public void Dispose()
    {
        _stopwatch.Stop();
        Console.WriteLine("Method finished in: " + _stopwatch.ElapsedMilliseconds + "ms");
    }
}