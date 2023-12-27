namespace AnalysisParalysis.Exceptions;

public class UserNotConnectedException : Exception
{
    public UserNotConnectedException() { }

    public UserNotConnectedException(string message) : base(message) { }

    public UserNotConnectedException(string message, Exception inner) : base(message, inner) { }
}