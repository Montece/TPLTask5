using TPLTask4;
using TPLTask5;

Console.Title = "TPLTask 5";

var allString = new List<string>();
using var catalog = new Catalog(Console.WriteLine);

while (true)
{
    Console.Write("Enter string: ");
    var text = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(text))
    {
        break;
    }

    allString.Add(text);
}

var sleepSort = new SleepSort(Console.WriteLine, catalog);

sleepSort.Do(allString.ToArray());

Console.WriteLine("Press ENTER to exit...");
Console.ReadLine();