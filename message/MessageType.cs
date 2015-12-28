namespace Compiler.message
{
    public enum MessageType
    {
        SourceLine,
        SyntaxError,
        ParserSummary,
        InterpreterSummary,
        CompilerSummary,
        Miscellaneous,
        Token,
        Assign,
        Fetch,
        BreakPoint,
        RuntimeError,
        Call,
        Return,
    }
}