namespace TPLTask.Logging.Loggers;

public class ConsoleLogger : ILogger
{
    public void Write(object message)
    {
        Console.WriteLine(message);
    }
}