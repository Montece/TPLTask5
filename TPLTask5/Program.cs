using TPLTask.Logging.Loggers;
using TPLTask5;

Console.Title = "TPLTask 5";

if (args.Length == 0)
{
    Console.WriteLine("Use args as a strings' array!");
}
else
{
    var sleepSort = new SleepSort(new ConsoleLogger(), Console.Out);

    sleepSort.Do(args);
}

Console.WriteLine("Press ENTER to exit...");
Console.ReadLine();