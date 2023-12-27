namespace AnalysisParalysis.Exceptions;

public class NoGamesFoundException : Exception
{
    public NoGamesFoundException() { }

    public NoGamesFoundException(string message) : base(message) { }

    public NoGamesFoundException(string message, Exception inner) : base(message, inner) { }
}