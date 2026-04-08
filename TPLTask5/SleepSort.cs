using TPLTask4;

namespace TPLTask5;

internal sealed class SleepSort
{
    private const int MAX_ELEMENTS_COUNT = 100;
    private const int TIME_PROPORTIONALITY_COEFFICIENT = 100;

    private readonly Action<string> _logMethod;
    private readonly Catalog _catalog;

    internal SleepSort(Action<string> logMethod, Catalog catalog)
    {
        ArgumentNullException.ThrowIfNull(logMethod, nameof(logMethod));
        ArgumentNullException.ThrowIfNull(catalog, nameof(catalog));

        _logMethod = logMethod;
        _catalog = catalog;
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

        _catalog.Show();
    }

    private void ThreadSort(object? singleStringObject)
    {
        try
        {
            if (singleStringObject is not string singleString)
            {
                _logMethod("Input sort data is not string!");

                return;
            }

            Thread.Sleep(singleString.Length * TIME_PROPORTIONALITY_COEFFICIENT);

            _catalog.AddToTail(singleString);
        }
        catch (Exception ex)
        {
            _logMethod($"Error while sorting! {ex}");
        }
    }
}