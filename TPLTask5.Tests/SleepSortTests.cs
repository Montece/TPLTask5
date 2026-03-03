using TPLTask.Logging.Loggers;
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
        using var data = new MemoryStream();
        using var dataStream = new StreamWriter(data);

        var sleepSort = new SleepSort(new ConsoleLogger(), dataStream);

        sleepSort.Do(initialOrder);

        data.Seek(0, SeekOrigin.Begin);
        using var actualData = new StreamReader(data);
        var actualOrder = new List<string>();

        while (!actualData.EndOfStream)
        {
            var line = actualData.ReadLine();

            ArgumentNullException.ThrowIfNull(line);

            actualOrder.Add(line);
        }

        Assert.Equal(expectedOrder, actualOrder);
    }
}