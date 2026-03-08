using TPLTask.Logging;

namespace TPLTask5;

internal sealed class SleepSort
{
    private const int MAX_ELEMENTS_COUNT = 100;
    private const int TIME_PROPORTIONALITY_COEFFICIENT = 100;

    private readonly ILogger _logger;
    private readonly TextWriter _output;

    private readonly Lock _outputLock = new();

    internal SleepSort(ILogger logger, TextWriter output)
    {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(output, nameof(output));

        _logger = logger;
        _output = output;
    }

    internal void Do(string[] allStrings)
    {
        ArgumentNullException.ThrowIfNull(allStrings, nameof(allStrings));

        if (allStrings.Length > MAX_ELEMENTS_COUNT)
        {
            throw new ArgumentException($"The number of elements must be less than or equal to {MAX_ELEMENTS_COUNT}");
        }

        var threads = new Thread[allStrings.Length];

        for (var i = 0; i < threads.Length; i++)
        {
            threads[i] = new(ThreadSort);

            threads[i].Start(allStrings[i]);
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }
    }

    private void ThreadSort(object? singleStringObject)
    {
        try
        {
            if (singleStringObject is not string singleString)
            {
                _logger.Write("Input sort data is not string!");

                return;
            }

            Thread.Sleep(singleString.Length * TIME_PROPORTIONALITY_COEFFICIENT);

            lock (_outputLock)
            {
                _output.WriteLine(singleString);
                _output.Flush();
            }
        }
        catch (Exception ex)
        {
            _logger.Write($"Error while sorting! {ex}");
        }
    }
}