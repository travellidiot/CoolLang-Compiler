namespace Compiler.message.cool
{
    public enum CoolErrorCode
    {
        TooManyErrors,
        InvalidCharacter,
        InvalidAssignment,
        InvalidConstant,
        InvalidSyntax,
        IOError,
        UnExpectedToken,
        UnExpectedEof,

        UndefinedType,
        RedefineVar,
        TypeNotMatch,
    }
}