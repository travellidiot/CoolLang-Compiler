namespace Compiler.message
{
    public enum MessageType
    {
        SourceLine,
        SyntaxError,
        SemanticsError,
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