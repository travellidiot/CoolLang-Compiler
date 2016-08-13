namespace Compiler.message.cool
{
    public enum ErrorCode
    {
        TooManyErrors,
        // syntax error
        InvalidCharacter,
        InvalidAssignment,
        InvalidConstant,
        InvalidSyntax,
        IOError,
        UnExpectedToken,
        UnExpectedEof,

        // semantics error
        UndefinedType,
        UndefinedVariable,
        UndefinedMethod,
        RedefineType,
        RedefineVariable,
        RedefineMethod,
        BadAssignment,
        BadNegation,
        BadBinaryOperaton,
        BadNotExpression,
        BadComparation,
        NotSuperType,
        CondNotBoolean,
    }
}