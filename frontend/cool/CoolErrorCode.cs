using System;

namespace Compiler.frontend.cool
{
    public enum CoolErrorCode
    {
        InvalidCharacter,
        InvalidAssignment,
        InvalidConstant,
        TooManyErrors,
        IOError,
        UnExpectedToken
    }
}