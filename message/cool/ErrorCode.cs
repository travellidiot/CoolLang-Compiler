namespace Compiler.message.cool
{
    public enum ErrorCode
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
        UndefinedVariable,
        UndefinedMethod,
        RedefineType,
        RedefineVariable,
        RedefineMethod,
        TypeNotMatch,
    }
}