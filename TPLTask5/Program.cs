using TPLTask4;
using TPLTask5;

Console.Title = "TPLTask 5";

if (args.Length == 0)
{
    Console.WriteLine("Use args as a strings' array!");
}
else
{
    using var catalog = new Catalog(Console.WriteLine);
    var sleepSort = new SleepSort(Console.WriteLine, catalog);

    sleepSort.Do(args);
}

Console.WriteLine("Press ENTER to exit...");
Console.ReadLine();