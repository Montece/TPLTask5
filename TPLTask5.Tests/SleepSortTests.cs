using TPLTask4;
using Xunit;

namespace TPLTask5.Tests;

public class SleepSortTests
{
    [Theory]
    [InlineData
    (
        new[] { "1", "11", "111111111", "11", "11111111111111111111", "111", "11111", "1111" },
        new[] { "1", "11", "11", "111", "1111", "11111", "111111111", "11111111111111111111" })]
    public void Do_UnsortedArray_SortedArray(string[] initialOrder, string[] expectedOrder)
    {
        var actualOrder = new List<string>();
        var logLock = new Lock();

        using var catalog = new Catalog(text =>
        {
            lock (logLock)
            {
                actualOrder.Add(text);
            }
        });

        var sleepSort = new SleepSort(Console.WriteLine, catalog);

        sleepSort.Do(initialOrder);

        Assert.Equal(expectedOrder, actualOrder);
    }
}